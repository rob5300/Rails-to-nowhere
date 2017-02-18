using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TestCurrentController : MonoBehaviour {
	private List<EngComponent> _circuits;
	public GameObject _pcb;
	// Use this for initialization
	void Start () {
		_circuits = GetComponentsInChildren<EngComponent>().ToList();
	}
	
	// Update is called once per frame
	void Update () {
		if (_circuits.Where(x => x._connected == true).Count() == _circuits.Count)
		{
			Destroy(_pcb);
		}
	}
}
