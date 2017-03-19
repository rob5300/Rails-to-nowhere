using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Carriage))]
public class CarriagePuzzleController : MonoBehaviour {

    Carriage carriage;
    public delegate void PuzzleEvent(PuzzleType puzzleType);
    public event Carriage.CarriageEvent AllPuzzlesCompleted;

    public int Puzzle3DTotal = 1;
    public int Puzzle2DTotal = 1;
    public int Completed3DPuzzles = 0;
    public int Completed2DPuzzles = 0;
    bool _allpuzzle2DDone = false;
    bool _allpuzzle3DDone = false;

    public enum PuzzleType { T3D, T2D };

    public void Start() {
        carriage = GetComponent<Carriage>();
    }

    public void OnPuzzleCompleted(PuzzleType type) {
        if(Puzzle2DTotal == 0) {
            _allpuzzle2DDone = true;
        }
        if (Puzzle3DTotal == 0) {
            _allpuzzle3DDone = true;
        }

        if (type == PuzzleType.T2D) {
            if ((Completed2DPuzzles + 1) < Puzzle2DTotal) {
                Completed2DPuzzles++;
            }
            else {
                _allpuzzle2DDone = true;
            }
        }
        else if (type == PuzzleType.T3D) {
            if ((Completed3DPuzzles + 1) < Puzzle3DTotal) {
                Completed3DPuzzles++;
            }
            else {
                _allpuzzle3DDone = true;
            }
        }

        

        if (_allpuzzle3DDone && _allpuzzle2DDone) {
            AllPuzzlesCompleted.Invoke();
        }
    }

    
}
