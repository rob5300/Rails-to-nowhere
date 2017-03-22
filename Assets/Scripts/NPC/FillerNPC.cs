using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NPC/Filler NPC", 1)]
public class FillerNPC : NPC {

    public static List<FillerNPC> FillerNPCs = new List<FillerNPC>();

    public static void LoadFillerNPCSDUMMY() {
        FillerNPCs.Add(GameObject.Find("TestFillerNPC").GetComponent<FillerNPC>());
    }

    public FillerNPC() {
        TestProp = new List<GameObject>();
    }

    public List<GameObject> TestProp { get; set; }

    public override List<string> GetSerialiseTargets() {
        List<string> props = base.GetSerialiseTargets();
        props.Add("TestProp");
        return props;
    }
}
