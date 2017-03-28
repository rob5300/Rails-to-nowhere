using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Player))]
public class NPCInteract : MonoBehaviour {

    public float ReachDistance;
    public float NPCHitDelay;

    Vector3 raycastStart;
    Vector3 fowardDirection;
    RaycastHit npcHit;
    float timeSinceHit = 0f;

    void Start() {
        Player.player.InteractEvent += OnRaycastHit;
    }

    void OnRaycastHit(GameObject hit) {
        NPC npc = hit.gameObject.GetComponent<NPC>();
        if (npc == null) npc = hit.gameObject.GetComponentInParent<NPC>();
        if (npc != null && !UI.MenuOpen) {
            if (npc.Interactable && npc.InitialDialogueNodeKey != null) UI.NewDialogueConversation(npc);
        }
    }

    private void Update() {
        raycastStart = Camera.main.transform.position;
        fowardDirection = Camera.main.transform.TransformDirection(Vector3.forward);

        //Ability to kill npcs.
        if (Input.GetMouseButtonDown(0)) {
            if (Physics.Raycast(raycastStart, fowardDirection, out npcHit, ReachDistance)) {
                if (!UI.MenuOpen) {
                    NPC npc = npcHit.collider.GetComponent<NPC>();
                    if (npc == null) npc = npcHit.collider.GetComponentInParent<NPC>();
                    if (npc != null && (Time.time - timeSinceHit) > NPCHitDelay) {
                        //If we have hit an npc and the time delay is passed, damage them.
                        npc.Damage(1);
                        timeSinceHit = Time.time;
                    }
                }
            }
        }
    }
}

