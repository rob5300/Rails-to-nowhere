using UnityEngine;
using UnityEngine.Events;

public class CogPuzzle : MonoBehaviour {

    public CogMount[] CogMounts;
    public UnityEvent WinEvent;

    void Start() {
        foreach (CogMount mount in CogMounts) {
            if (mount.ParentPuzzle == null) mount.ParentPuzzle = this;
        }
        WinEvent.AddListener(GenericWin);
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
        WinEvent.Invoke();
    }

    void PuzzleError() {
        Debug.Log("Puzzle failed!");
    }

    public void GenericWin() {
        Debug.Log("Puzzle complete!");
    }
}
