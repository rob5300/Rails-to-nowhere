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

    public delegate void CarriageEvent(Carriage carriage);
    public event CarriageEvent PuzzlesComplete;

    public void OpenDoor() {
        CarriageDoor.Unlock();
    }

    public void PuzzlesDone() {

    }
}
