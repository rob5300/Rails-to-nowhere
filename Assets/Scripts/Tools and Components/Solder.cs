using UnityEngine;
using System.Collections;

public class Solder : EngComponent
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay2D()
	{
		_connected = true;
	}
}
