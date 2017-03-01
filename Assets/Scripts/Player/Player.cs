using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public static Player player;

    public float InteractDistance;
    public MonoBehaviour Controller;

    public delegate void InteractEvent(GameObject eventObject);
    public static event InteractEvent interactEvent;

    RaycastHit _hit;

    void Start() {
        //Assign the static variable player to the only instance of this class that should exist.
        if (!player) player = this;
        else Debug.LogError("Multiple player instances on objects: " + player.name + ", " + name);

        //We ensure the players is on the ignore raycast layer to prevent issues. This can be removed if raycaysts ignore the players layer.
        gameObject.layer = 2;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out _hit,InteractDistance)) {
                if(_hit.collider.gameObject != null) interactEvent.Invoke(_hit.transform.gameObject);
            }
        }
    }
}
