using UnityEngine;
using System.Collections.Generic;

[AddComponentMenu("NPC/Story NPC", 0)]
public class StoryNPC : NPC {

    public static List<StoryNPC> StoryNPCs = new List<StoryNPC>();
    private GameObject carriage;
    private bool essential = false;

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
