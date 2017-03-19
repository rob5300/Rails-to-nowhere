using UnityEngine;
using System.Collections;

public class PuzzleEventSubscriber : MonoBehaviour {

    public CarriagePuzzleController puzzleController;

    void Awake() {
        CogPuzzle puzzle = GetComponent<CogPuzzle>();
        if (puzzle && puzzleController) puzzle.PuzzleComplete += puzzleController.OnPuzzleCompleted;

        Destroy(this);
    }
}
