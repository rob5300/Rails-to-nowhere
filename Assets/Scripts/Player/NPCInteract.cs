using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Player))]
public class NPCInteract : MonoBehaviour {

    void Start() {
        Player.interactEvent += OnRaycastHit;
    }

    void OnRaycastHit(GameObject hit) {
        StoryNPC npc = hit.gameObject.GetComponent<StoryNPC>();
        if (npc != null && !UI.MenuOpen) {
            if (npc.Interactable && npc.InitialDialogueNodeName != null) UI.DialogueConversation(DialogueController.GetNode(npc.InitialDialogueNodeName)); 
        }
    }
}

