using System;
using UnityEngine;

public class StoryNPC : NPC {

	public bool Essential = false;

    public override void Damage(float damage) {
        if (Health - damage <= 0) {
            Health = 0;
        }
        else {
            Health -= damage;
        }
    }

    public override void Interact() {
        Debug.Log("Interacted with: " + Name);
    }
}
