using System;
using UnityEngine;

public class StoryNPC : NPC {

	public bool Essential = false;
    public string InitialDialogueNodeName;
    public bool HasSpeach { get {
            if(InitialDialogueNodeName == null) {
                return false;
            }
            else {
                return true;
            }
        }}

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

    public override void Interact() {
        Debug.Log("Interacted with: " + Name);
    }
}
