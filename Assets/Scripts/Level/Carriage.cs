using UnityEngine;

public class Carriage : MonoBehaviour {

    public Transform FrontMountPoint;
    public Transform RearMountPoint;

    public enum CarriageType {Story, Filler};
    public CarriageType Type;

    public Transform NPCPosition;
    public Transform PuzzlePosition;
}
