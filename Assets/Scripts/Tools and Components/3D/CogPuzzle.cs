using System.Collections.Generic;
using UnityEngine;

public class CogPuzzle : Puzzle {

    public static List<GameObject> PuzzleList;

    public static void Load3DPuzzlePrefabs() {
        PuzzleList = new List<GameObject>(){
            Resources.Load<GameObject>("3DPuzzles/Cog Puzzle"),
            Resources.Load<GameObject>("3DPuzzles/Cog Puzzle Tri"),
            Resources.Load<GameObject>("3DPuzzles/Cog Puzzle Tri 2")
        };
    }

    public CogMount[] CogMounts;

    void Start() {
        foreach (CogMount mount in CogMounts) {
            if (mount.ParentPuzzle == null) mount.ParentPuzzle = this;
        }
    }

    public void CheckPuzzle() {
        int totalMounts = CogMounts.Length;
        int correctMounts = 0;
        foreach (CogMount mount in CogMounts) {
            if (mount.HasCog) {
                if (mount.AttachedCog.Size == mount.Size) {
                    correctMounts++;
                    mount.CorrectMount();
                }
                else {
                    mount.IncorrectMount();
                }
            }
            else {
                mount.IncorrectMount();
            }
        }
        if (correctMounts == totalMounts) {
            PuzzleDone(CarriagePuzzleController.PuzzleType.T3D);
            UI.ShowMessage("The cogs all fit and turn.");
        }
        else{
            PuzzleError();
        }
    }

    void PuzzleError() {
        UI.ShowMessage("The cogs don't seem to be in the correct places...");
    }
}
