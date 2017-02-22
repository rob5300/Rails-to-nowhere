using UnityEngine;
using System.Collections;

public class NPCInteract : MonoBehaviour {

    float ReachDistance = 5;

    Vector3 raycastStart;
    Vector3 fowardDirection;

    RaycastHit hitNPC;

    void Update() {
        if (Input.GetKeyDown(KeyCode.F)) {
            raycastStart = Camera.main.transform.position;
            fowardDirection = Camera.main.transform.TransformDirection(Vector3.forward);

            if (Physics.Raycast(raycastStart, fowardDirection, out hitNPC, ReachDistance)) {
                StoryNPC npc = hitNPC.transform.GetComponent<StoryNPC>();
                if (npc != null) {
                    if (npc.Interactable) {

                    }
                }
            }
        }
    }
}

