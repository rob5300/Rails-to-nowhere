using UnityEngine;
using System.Collections;

public class Cheat : MonoBehaviour {

	void Update () {
        if (Input.GetKeyDown(KeyCode.V)) {
            Player.player.transform.Translate(Vector3.forward * 5, Space.Self);
        }
        if (Input.GetKeyDown(KeyCode.N)) {
            Carriage.CarriageList[Carriage.CarriageList.Count - 1].SetActive(true);
            Player.player.transform.position = new Vector3(217f, 2.2f, 0.27f);
        }
	}
}
