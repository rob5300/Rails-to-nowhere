using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

[AddComponentMenu("NPC/Story NPC", 0)]
public class StoryNPC : NPC {

    public static List<StoryNPC> StoryNPCs = new List<StoryNPC>();

    public delegate void StoryNPCEvent();
    public event StoryNPCEvent EnablePuzzles;

    [SerializeField]
    private bool essential = false;
    [SerializeField]
    private string enablePuzzlesNode;
    [SerializeField]
    private GameObject carriage;
    public bool Essential {
        get {
            return essential;
        }

        set {
            essential = value;
        }
    }

    public GameObject Carriage {
        get {
            return carriage;
        }

        set {
            carriage = value;
        }
    }

    public string EnablePuzzlesNode {
        get {
            return enablePuzzlesNode;
        }

        set {
            enablePuzzlesNode = value;
        }
    }

    public static void LoadStoryNPCSDUMMY() {
        StoryNPCs.Add(GameObject.Find("TestStoryNPC").GetComponent<StoryNPC>());
    }

    public static void LoadStoryNPCs() {
        UnityXMLSerialiser<StoryNPC> serialiser = new UnityXMLSerialiser<StoryNPC>();
        List<string> result = Directory.GetFiles(Application.streamingAssetsPath.Replace('/', '\\')).ToList();
        foreach (string path in result) {
            FileInfo info = new FileInfo(path);
            if (info.FullName.Contains("StoryNPC") && info.Extension != ".meta") {
				var resultObj = serialiser.DeserialiseXML(info);
				StoryNPCs.Add(resultObj);
            }

        }
    }

    public void InvokeEnablePuzzles() {
        EnablePuzzles.Invoke();
    }

    public override void Damage(float damage)
	{
		if (Health - damage <= 0)
		{
			Health = 0;
			if (!Essential)
			{
				Die();
			}
		}
		else {
			Health -= damage;
		}
	}

}
