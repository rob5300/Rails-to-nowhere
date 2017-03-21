using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Player))]
public class NPCInteract : MonoBehaviour {

    void Start() {
        Player.InteractEvent += OnRaycastHit;
    }

    void OnRaycastHit(GameObject hit) {
        StoryNPC npc = hit.gameObject.GetComponent<StoryNPC>();
        if (npc != null && !UI.MenuOpen) {
            if (npc.Interactable && npc.InitialDialogueNodeName != null) UI.DialogueConversation(DialogueController.GetNode(npc.InitialDialogueNodeName), npc.MemoryItemKey, npc.MemoryResponseTotal);
        }
    }
}

