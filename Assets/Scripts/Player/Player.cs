using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public static Player player;

    void Start() {
        //Assign the static variable player to the only instance of this class that should exist.
        if (!player) player = this;
        else Debug.LogError("Multiple player instances on objects: " + player.name + ", " + name);
    }
}
