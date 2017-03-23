using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NPC/Filler NPC", 1)]
public class FillerNPC : NPC {

    public static List<FillerNPC> FillerNPCs = new List<FillerNPC>();

    public delegate void FillerNPCEvent();
    public event FillerNPCEvent DialogueDoorOpen;

    public string OpenDoorNode { get; set; }

    public FillerNPC()
	{
		TestProp = new List<GameObject>();
	}
    public static void LoadFillerNPCSDUMMY() {
        FillerNPCs.Add(GameObject.Find("TestFillerNPC").GetComponent<FillerNPC>());
    }

    public List<GameObject> TestProp { get; set; }

    public void InvokeDialogueDoorOpen() {
        DialogueDoorOpen.Invoke();
    }
}
