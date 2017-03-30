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
    private static string stringCode;
    private static int codeDigitsGivenOut = 0;

    public static int Code {
        get {
            return code;
        }
    }

    public static void GenerateKeyCode() {
        System.Random rand = new System.Random();
        code = rand.Next(9999);
        stringCode = code.ToString();
        //Incase the number is not 4 digits long, we add on zeroes.
        //An alternative is to ensure the random number is between 1000 and 9999.
        if (code < 1000) {
            stringCode = "0" + code.ToString();
        }
        if(code < 100) {
            stringCode = "00" + code.ToString();
        }
        if(code < 10) {
            stringCode = "000" + code.ToString();
        }
        Debug.Log("Code: " + stringCode);
    }

    //returns the next digit to add to a memory from the code.
    public static int GetNextDigit() {
        codeDigitsGivenOut++;
        if (codeDigitsGivenOut > 4) codeDigitsGivenOut = 5;
        //I cheat and return the value after incrementing the codeDigitsGivenOut int, soo i remove 1 to compensate to return the correct digit.
        return Convert.ToInt32(stringCode.ToCharArray()[codeDigitsGivenOut - 1].ToString());
        //We must convert back to a string from char to ensure a literal conversion.
    }

    public static EndingType GetEndingType() {
		//Calculate total percentage.
		decimal npcResult = (NPCsAlive / TotalNPCs) * 100;
		decimal puzzleResult = (PuzzlesComplete / TotalPuzzles) * 100;
		decimal memoryResult = (MemoriesGained / 4) * 100;
		decimal progressionResult = (npcResult + puzzleResult + memoryResult) / 3;

        if(progressionResult >= 99) {
            return EndingType.True;
        }
		else if(progressionResult == 0) {
			return EndingType.Bad;
		}
		else { 
            return EndingType.Neutral;
        }

    }

    public static void SetTotalValues(int totalNPCs, int totalMemories, int totalPuzzles) {
        TotalNPCs = totalNPCs;
        NPCsAlive = totalNPCs;
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
