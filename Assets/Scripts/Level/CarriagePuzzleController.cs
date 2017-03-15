using UnityEngine;
using System.Collections;

public class CarriagePuzzleController : MonoBehaviour {

    public Carriage carriage;

	public void Start() {
        carriage = GetComponent<Carriage>();
    }

}
