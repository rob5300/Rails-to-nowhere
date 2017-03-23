using UnityEngine;
using System.Collections.Generic;

public class Carriage : MonoBehaviour {

    public Transform FrontMountPoint;
    public Transform RearMountPoint;

    public enum CarriageType {Story, Filler};
    public CarriageType Type;

    public Door CarriageDoor;

    public Transform NPCPosition;
    public Transform Puzzle3DPosition;
    public Transform Puzzle2DPosition;

    private GameObject Puzzle2D;
    private GameObject Puzzle3D;

    public CarriagePuzzleController PuzzleController;

    public delegate void CarriageEvent();
    //Used when ALL puzzles are done.

    public void Start() {
        SetupExtraEvents();
    }

    public void AllPuzzlesComplete() {
        UI.ShowMessage("The door was unlocked!");
        CarriageDoor.Unlock();
    }

    public void OnNPCDeath(NPC npc) {
        UI.ShowMessage(npc.name + " died. The door mysteriously opened itself...");
        CarriageDoor.Unlock();
    }

    public void Place2DPuzzle(GameObject puzzleToPlace) {
        GameObject puzzleOpener = Resources.Load<GameObject>("2DPuzzles/Electrical Puzzle Open");
        GameObject placedOpener = (GameObject)Instantiate(puzzleOpener, Puzzle2DPosition.position, Puzzle2DPosition.rotation);
        placedOpener.transform.position += new Vector3(0, 5, 0);
        GameObject placedPuzzle = Instantiate(puzzleToPlace);
        placedOpener.GetComponent<Puzzle2D>().Puzzle = placedPuzzle;
        placedPuzzle.SetActive(false);

        Puzzle2D = puzzleToPlace;
        puzzleToPlace.SetActive(false);
    }

    public void Place3DPuzzle(GameObject puzzle3D) {
        GameObject puzzle = (GameObject)Instantiate(puzzle3D, Puzzle3DPosition.position, Puzzle3DPosition.rotation);
        puzzle.GetComponent<CogPuzzle>().PuzzleComplete += PuzzleController.OnPuzzleCompleted;

        Puzzle3D = puzzle3D;
        Puzzle3D.SetActive(false);
    }

    public void PlaceCarriageNPC(StoryNPC npc) {
        GameObject newNPC = (GameObject)Instantiate(npc.ModelPrefab, NPCPosition.position, NPCPosition.rotation);
        newNPC.transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
        StoryNPC newStoryNPC = newNPC.AddComponent<StoryNPC>();
        newStoryNPC.Name = npc.Name;
        newStoryNPC.MemoryItemKey = npc.MemoryItemKey;
        newStoryNPC.MemoryResponseTotal = npc.MemoryResponseTotal;
        newStoryNPC.InitialDialogueNodeName = npc.InitialDialogueNodeName;
        newStoryNPC.Health = npc.Health;
        newStoryNPC.Interactable = npc.Interactable;
        newStoryNPC.NPCDeath += OnNPCDeath;
        newStoryNPC.EnablePuzzlesNode = npc.EnablePuzzlesNode;
        newStoryNPC.EnablePuzzles += ShowPuzzles;
    }

    public void PlaceCarriageNPC(FillerNPC npc) {
        GameObject newNPC = (GameObject)Instantiate(npc.ModelPrefab, NPCPosition.position, NPCPosition.rotation);
        newNPC.transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
        FillerNPC newFillerNPC = newNPC.AddComponent<FillerNPC>();
        newFillerNPC.Name = npc.Name;
        newFillerNPC.MemoryItemKey = npc.MemoryItemKey;
        newFillerNPC.MemoryResponseTotal = npc.MemoryResponseTotal;
        newFillerNPC.InitialDialogueNodeName = npc.InitialDialogueNodeName;
        newFillerNPC.Health = npc.Health;
        newFillerNPC.Interactable = npc.Interactable;
        newFillerNPC.NPCDeath += OnNPCDeath;
        newFillerNPC.OpenDoorNode = npc.OpenDoorNode;
        newFillerNPC.DialogueDoorOpen += AllPuzzlesComplete;
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
