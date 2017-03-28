using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EndTrigger : MonoBehaviour {

    bool activated = false;

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player" && !activated) {
            activated = true;
            UI.Ending();
        }
    }
}
