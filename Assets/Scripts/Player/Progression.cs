using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Progression {

    public enum EndingType {True, Neutral, Bad};

    public static int TrueEndingThresholdPercentage = 99;
    public static int BadEndingThresholdPercentage = 0;

    public static int NPCsAlive = 0;
    public static int TotalNPCs = 0;
    public static int MemoriesGained = 0;
    public static int TotalMemories = 0;
    public static int PuzzlesComplete = 0;
    public static int TotalPuzzles = 0;
    public static int TotalProgressPercantage;
    private static int code;

    public static int Code {
        get {
            return code;
        }
    }

    public static void GenerateKeyCode() {
        Random rand = new Random();
        code = rand.Next(9999);
    }

    

    public static EndingType GetEndingType() {
        //Calculate total percentage.

        if(TotalProgressPercantage >= TrueEndingThresholdPercentage) {
            return EndingType.True;
        }
        else if(TotalProgressPercantage > BadEndingThresholdPercentage) {
            return EndingType.Neutral;
        }
        else {
            return EndingType.Bad;
        }
    }

    public static void SetTotalValues(int totalNPCs, int totalMemories, int totalPuzzles) {
        TotalNPCs = totalNPCs;
        TotalMemories = totalMemories;
        TotalPuzzles = totalPuzzles;
    }
}
