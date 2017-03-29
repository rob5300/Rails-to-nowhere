using UnityEngine;
using System.Collections.Generic;
[RequireComponent(typeof(BoxCollider))]
public class InnerDialogueTrigger : MonoBehaviour {

    public List<string> ThoughtText;
    public string name1;
    public string name2;
    public bool Activated = false;

    private void OnTriggerEnter(Collider other) {
        if (!Activated && other.tag == "Player") {
            UI.InnerDialogue(ThoughtText, name1, name2);
            Activated = true;
        }
    }
}
