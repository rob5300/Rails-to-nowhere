using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Player))]
public class NPCInteract : MonoBehaviour {

    void Start() {
        Player.InteractEvent += OnRaycastHit;
    }

    void OnRaycastHit(GameObject hit) {
        NPC npc = hit.gameObject.GetComponent<NPC>();
        if (npc != null && !UI.MenuOpen) {
            if (npc.Interactable && npc.InitialDialogueNodeName != null) UI.NewDialogueConversation(npc);
        }
    }
}

