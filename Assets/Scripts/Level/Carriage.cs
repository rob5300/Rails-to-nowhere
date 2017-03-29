using UnityEngine;
using System.Collections.Generic;

public class Carriage : MonoBehaviour {

    public static List<GameObject> CarriageList = new List<GameObject>();
    public static Carriage StartCarriage;
    public static GameObject LastCarriageWall;
    public static GameObject previousDestroyFrontWallandDoor;

    public Transform FrontMountPoint;
    public Transform RearMountPoint;

    public enum CarriageType {Start, Story, Filler, Ending};
    public CarriageType Type;

    public Door CarriageDoor;
    public GameObject FrontWallandDoor;

    public Transform NPCPosition;
    public Transform Puzzle3DPosition;
    public Transform Puzzle2DPosition;

    private GameObject Puzzle2D;
    private GameObject Puzzle3D;

    public CarriagePuzzleController PuzzleController;

    public delegate void CarriageEvent();
    //Used when ALL puzzles are done.

    public static void DisableLastCarraige() {
        CarriageList[0].GetComponent<Carriage>().CloseDoorAndDisableSelf();
        CarriageList.RemoveAt(0);
    }

    public static void EnableNextCarriage() {
        if(CarriageList.Count > 0) CarriageList[1].SetActive(true);
    }

    public void Awake() {
        if (Type == CarriageType.Start) {
            StartCarriage = this;
        }
    }

    public void Start() {
        if(Type != CarriageType.Ending) SetupExtraEvents();
    }

    public void CloseDoorAndDisableSelf() {
        CarriageDoor.Close();
        LastCarriageWall = FrontWallandDoor;
        if(previousDestroyFrontWallandDoor != null) {
            Destroy(previousDestroyFrontWallandDoor);
        }
        previousDestroyFrontWallandDoor = LastCarriageWall;
        FrontWallandDoor.transform.parent = null;
        Destroy(gameObject, 2f);
    }

    public void AllPuzzlesComplete() {
        UI.ShowMessage("The door was unlocked!");
        CarriageDoor.Open();
    }

    public void OnNPCDeath(NPC npc) {
        UI.ShowMessage(npc.Name + " died. The door mysteriously opened itself...");
        CarriageDoor.Open();
    }

    public void Place2DPuzzle(GameObject puzzleToPlace) {
        GameObject puzzleOpener = Resources.Load<GameObject>("2DPuzzles/Electrical Puzzle Open");
        GameObject placedOpener = (GameObject)Instantiate(puzzleOpener, Puzzle2DPosition.position, Puzzle2DPosition.rotation);
        placedOpener.transform.position += new Vector3(0, 0.5f, 0);
        GameObject placedPuzzle = Instantiate(puzzleToPlace);
        placedOpener.GetComponent<Puzzle2DEnvironmentLoader>().Puzzle = placedPuzzle;
        //Checks for the component in children as the component does not live on the prefab parent.
		placedPuzzle.GetComponentInChildren<TwoDimensionalPuzzle>().PuzzleComplete += PuzzleController.OnPuzzleCompleted;
		placedPuzzle.SetActive(false);
        Puzzle2D = placedOpener;
        placedOpener.SetActive(false);
        placedOpener.transform.parent = transform;
    }

    public void Place3DPuzzle(GameObject puzzle3D) {
        GameObject puzzle = (GameObject)Instantiate(puzzle3D, Puzzle3DPosition.position, Puzzle3DPosition.rotation);
        puzzle.GetComponent<CogPuzzle>().PuzzleComplete += PuzzleController.OnPuzzleCompleted;

        Puzzle3D = puzzle;
        Puzzle3D.SetActive(false);
        puzzle.transform.parent = transform;
    }

    public void PlaceCarriageNPC(StoryNPC npc) {
        GameObject newNPC = (GameObject)Instantiate(npc.ModelPrefab, NPCPosition.position, NPCPosition.rotation);
        StoryNPC newStoryNPC = newNPC.AddComponent<StoryNPC>();
        newStoryNPC.Name = npc.Name;
        newStoryNPC.MemoryItemKey = npc.MemoryItemKey;
        newStoryNPC.MemoryResponseTotal = npc.MemoryResponseTotal;
        newStoryNPC.InitialDialogueNodeKey = npc.InitialDialogueNodeKey;
        newStoryNPC.Health = npc.Health;
        newStoryNPC.Interactable = npc.Interactable;
        newStoryNPC.NPCDeath += OnNPCDeath;
        newStoryNPC.EnablePuzzlesNode = npc.EnablePuzzlesNode;
        newStoryNPC.EnablePuzzles += ShowPuzzles;
        newStoryNPC.NPCDeath += OnNPCDeath;

        newStoryNPC.transform.parent = transform;
    }

    public void PlaceCarriageNPC(FillerNPC npc) {
        GameObject newNPC = (GameObject)Instantiate(npc.ModelPrefab, NPCPosition.position, NPCPosition.rotation);
        FillerNPC newFillerNPC = newNPC.AddComponent<FillerNPC>();
        newFillerNPC.Name = npc.Name;
        newFillerNPC.MemoryItemKey = npc.MemoryItemKey;
        newFillerNPC.MemoryResponseTotal = npc.MemoryResponseTotal;
        newFillerNPC.InitialDialogueNodeKey = npc.InitialDialogueNodeKey;
        newFillerNPC.Health = npc.Health;
        newFillerNPC.Interactable = npc.Interactable;
        newFillerNPC.NPCDeath += OnNPCDeath;
        newFillerNPC.OpenDoorNode = npc.OpenDoorNode;
        newFillerNPC.DialogueDoorOpen += AllPuzzlesComplete;
        newFillerNPC.NPCDeath += OnNPCDeath;

        newFillerNPC.transform.parent = transform;
    }

    public void SetupExtraEvents() {
        PuzzleController.AllPuzzlesCompleted += AllPuzzlesComplete;
    }

    public void ShowPuzzles() {
        UI.ShowMessage("Puzzles have appeared in the Carriage.");
        Puzzle3D.SetActive(true);
        Puzzle2D.SetActive(true);
    }
}
