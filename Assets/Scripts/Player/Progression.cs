using System;
using UnityEngine;

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
    private static int codeDigitsGivenOut = 0;

    public static int Code {
        get {
            return code;
        }
    }

    public static void GenerateKeyCode() {
        System.Random rand = new System.Random();
        code = rand.Next(9999);
        Debug.Log("Code: " + code);
    }

    //returns the next digit to add to a memory from the code.
    public static int GetNextDigit() {
        codeDigitsGivenOut++;
        if (codeDigitsGivenOut > 4) codeDigitsGivenOut = 5;
        //I cheat and return the value after incrementing the codeDigitsGivenOut int, soo i remove 1 to compensate to return the correct digit.
        string stringcode = Code.ToString();
        char[] codechararray = stringcode.ToCharArray();

        int toreturn = Convert.ToInt32(codechararray[codeDigitsGivenOut - 1].ToString());
        return toreturn;
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

    public static string GetEndingBranchDialogueNode() {
        if(GetEndingType() == EndingType.True) {
            return "ending.true";
        }
        else if (GetEndingType() == EndingType.Bad) {
            return "ending.bad";
        }
        else {
            //Neutral ending
            return "ending.neutral";
        }
    }
}
