using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class CarriageDisableTrigger : MonoBehaviour {

    bool activated = false;

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player" && !activated) {
            Carriage.DisableLastCarraige();
            activated = true;
        }
    }
}
