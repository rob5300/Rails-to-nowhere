using UnityEngine;
using System.Collections;

public class Cheat : MonoBehaviour {

	void Update () {
        if (Input.GetKeyDown(KeyCode.V)) {
            Player.player.transform.Translate(Vector3.forward * 5, Space.Self);
        }
	}
}
