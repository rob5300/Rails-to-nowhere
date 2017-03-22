using System;
using UnityEngine;

public class Puzzle : MonoBehaviour {

    public event CarriagePuzzleController.PuzzleEvent PuzzleComplete;

    public void PuzzleDone(CarriagePuzzleController.PuzzleType puzzleType) {
        PuzzleComplete.Invoke(puzzleType);
    }
}
