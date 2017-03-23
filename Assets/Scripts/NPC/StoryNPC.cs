using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

[AddComponentMenu("NPC/Story NPC", 0)]
public class StoryNPC : NPC {

    public static List<StoryNPC> StoryNPCs = new List<StoryNPC>();
    [SerializeField]

    private bool essential = false;

    public bool Essential {
        get {
            return essential;
        }

        set {
            essential = value;
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
                StoryNPCs.Add(serialiser.DeserialiseXML(info));
            }

        }
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
