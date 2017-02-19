using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Solder : EngComponent
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		List<RaycastHit2D> result = Physics2D.CircleCastAll(transform.position, 0.180f, Vector2.zero, 0f).Where(x => x.transform.tag == "Circuit").ToList();
		if (result.Where(x => x.transform.GetComponent<EngComponent>()._connected == true).Any())
		{
			_connected = true;
		}
	}
}
