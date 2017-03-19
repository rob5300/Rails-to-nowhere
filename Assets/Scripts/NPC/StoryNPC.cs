using UnityEngine;
using System.Collections.Generic;

[AddComponentMenu("NPC/Story NPC", 0)]
public class StoryNPC : NPC {

    public static List<StoryNPC> StoryNPCs = new List<StoryNPC>();
    public bool Essential = false;

    public override void Damage(float damage) {
        if (Health - damage <= 0) {
            Health = 0;
            if (!Essential) {
                Die();
            }
        }
        else {
            Health -= damage;
        }
    }

}
