using System;
using UnityEngine;

[AddComponentMenu("NPC/Story NPC", 0)]
public class StoryNPC : NPC {

    public delegate void NPCEvent(StoryNPC npc);
    public event NPCEvent OpenDoor;

    public int MemoryResponseTotal = 0;
    public string MemoryItemKey = "memory.basic";

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

    public void AwardMemory() {
        Player.player.inventory.AddItem(MemoryItemKey, 1);
    }

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
