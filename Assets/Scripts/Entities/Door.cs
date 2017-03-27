using UnityEngine;

[AddComponentMenu("Entities/Door", 3)]
public class Door : Entity {

	public bool Locked = true;

    public void Open() {
        if (Locked) {
            GetComponent<Animation>().Play("Door Open");
            Locked = false;
            Carriage.EnableNextCarriage();
        }
    }

    public void Close() {
        if (!Locked) {
            GetComponent<Animation>().Play("Door Close");
            Locked = true;
        }
    }

    public override void OnInteract() {
        //Allow the player to open the door with other means?
    }
}
