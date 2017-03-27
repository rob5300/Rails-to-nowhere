using UnityEngine;
using System.Collections.Generic;
[RequireComponent(typeof(BoxCollider))]
public class InnerDialogueTrigger : MonoBehaviour {

    public List<string> ThoughtText;
    public string nameOfPersonTalking;
    public bool Activated = false;

    private void OnTriggerEnter(Collider other) {
        if (!Activated && other.tag == "Player") {
            UI.InnerDialogue(ThoughtText, nameOfPersonTalking);
            Activated = true;
        }
    }
}
