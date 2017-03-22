using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NPC/Filler NPC", 1)]
public class FillerNPC : NPC {

    public static List<FillerNPC> FillerNPCs = new List<FillerNPC>();

    public static void LoadFillerNPCSDUMMY() {
        FillerNPCs.Add(GameObject.Find("TestFillerNPC").GetComponent<FillerNPC>());
    }
}
