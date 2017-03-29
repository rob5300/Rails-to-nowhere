using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class Player : MonoBehaviour {

    public static Player player;
    public Inventory inventory = new Inventory(4);
    public float InteractDistance;
    public MonoBehaviour Controller;
    public BlurOptimized blurEffect;
    public Animator handsAnimator;

    public delegate void WorldEvent(GameObject eventObject);
    public event WorldEvent InteractEvent;

    RaycastHit _hit;

    void Awake() {
        //Assign the static variable player to the only instance of this class that should exist.
        if (!player) player = this;
        else Debug.LogError("Multiple player instances on objects: " + player.name + ", " + name);

        //We ensure the players is on the ignore raycast layer to prevent issues. This can be removed if raycaysts ignore the players layer.
        gameObject.layer = 2;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out _hit,InteractDistance)) {
                if(_hit.collider.gameObject != null) InteractEvent.Invoke(_hit.collider.gameObject);
            }
        }
    }
}
