using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TestCurrentController : MonoBehaviour {
	public List<EngComponent> _circuits;
	public GameObject _pcb;
	// Use this for initialization
	void Start () {
		_circuits = GetComponentsInChildren<EngComponent>().ToList();
	}
	
	// Update is called once per frame
	void Update () {
		if (_circuits.Where(x => x.name == "EndGoal" && x.Connected == true).Any())
		{
			Destroy(_pcb);
		}
	}
}
