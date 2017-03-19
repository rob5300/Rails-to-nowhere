using UnityEngine;
using UnityEngine.Events;

public class CogPuzzle : MonoBehaviour {

    public CogMount[] CogMounts;
    public event CarriagePuzzleController.PuzzleEvent PuzzleComplete;

    void Start() {
        foreach (CogMount mount in CogMounts) {
            if (mount.ParentPuzzle == null) mount.ParentPuzzle = this;
        }
    }

    public void CheckPuzzle() {
        foreach (CogMount mount in CogMounts) {
            if (mount.HasCog) {
                if (mount.AttachedCog.Size != mount.Size) {
                    PuzzleError();
                    return;
                }
            }
            else {
                PuzzleError();
                return;
            }
        }
        PuzzleDone();
    }

    void PuzzleError() {
        Debug.Log("Puzzle failed!");
    }

    public void PuzzleDone() {
        Debug.Log("Puzzle complete");
        PuzzleComplete.Invoke(CarriagePuzzleController.PuzzleType.T3D);
    }
}
