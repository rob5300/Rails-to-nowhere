using UnityEngine;

[AddComponentMenu("Entities/Door", 3)]
[RequireComponent(typeof(Rigidbody))]
public class Door : Entity {

	public bool Locked = true;

    private void Start() {
        if (Locked) {
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    public void Unlock() {
        if (Locked) {
            GetComponent<Rigidbody>().isKinematic = false;
            Locked = false;
        }
    }

    public override void OnInteract() {
        //Allow the player to open the door with other means?
    }
}
