using System.Collections.Generic;
using UnityEngine;

public class CogPuzzle : Puzzle {

    public static List<GameObject> PuzzleList;

    public static void Load3DPuzzlePrefabs() {
        PuzzleList = new List<GameObject>(){
            Resources.Load<GameObject>("3DPuzzles/Cog Puzzle"),
            Resources.Load<GameObject>("3DPuzzles/Cog Puzzle Tri")
        };
    }

    public CogMount[] CogMounts;

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
        PuzzleDone(CarriagePuzzleController.PuzzleType.T3D);
    }

    void PuzzleError() {
        UI.ShowMessage("Puzzle Failed");
    }

}
