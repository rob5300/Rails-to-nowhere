using UnityEngine;

public class Carriage : MonoBehaviour {

    public Transform FrontMountPoint;
    public Transform RearMountPoint;

    public enum CarriageType {Story, Filler};
    public CarriageType Type;

    public Door CarriageDoor;

    public Transform NPCPosition;
    public Transform PuzzlePosition;

    public CarriagePuzzleController PuzzleController;

    public delegate void CarriageEvent();
    //Used when ALL puzzles are done.

    public void Start() {
        AddNPC();
        AddPuzzles();
        SetupExtraEvents();
    }

    public void AllPuzzlesComplete() {
        CarriageDoor.Unlock();
    }
    
    public void AddNPC() {
        //Spawn npc
        //Set up events
    }

    public void AddPuzzles() {
        //Spawn puzzle
        //Set up events.
    }

    public void SetupExtraEvents() {
        PuzzleController.AllPuzzlesCompleted += AllPuzzlesComplete;
    }
}
