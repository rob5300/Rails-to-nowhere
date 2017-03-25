using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[AddComponentMenu("NPC/Filler NPC", 1)]
public class FillerNPC : NPC {

    public static List<FillerNPC> FillerNPCs = new List<FillerNPC>();

    public delegate void FillerNPCEvent();
    public event FillerNPCEvent DialogueDoorOpen;

    public string OpenDoorNode { get; set; }

    public FillerNPC()
	{
        CarriageObjects = new List<GameObject>();
	}

    public static void LoadStoryNPCs() {
        UnityXMLSerialiser<FillerNPC> serialiser = new UnityXMLSerialiser<FillerNPC>();
        List<string> result = Directory.GetFiles(Application.streamingAssetsPath.Replace('/', '\\')).ToList();
        foreach (string path in result) {
            FileInfo info = new FileInfo(path);
            if (info.FullName.Contains("FillerNPC") && info.Extension != ".meta") {
                var resultObj = serialiser.DeserialiseXML(info);
                FillerNPCs.Add(resultObj);
            }

        }
    }

    public List<GameObject> CarriageObjects { get; set; }

    public void InvokeDialogueDoorOpen() {
        DialogueDoorOpen.Invoke();
    }
}
