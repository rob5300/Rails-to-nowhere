using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TestCurrentController : MonoBehaviour {
	public List<EngComponent> _components;
	[SerializeField]
	private float _requiredPower;

	public float RequiredPower
	{
		get { return _requiredPower; }
		set { _requiredPower = value; }
	}

	// Use this for initialization
	void Start () {
		_components = GetComponentsInChildren<EngComponent>().ToList();
	}
	
	// Update is called once per frame
	void Update () {
        _components.RemoveAll(x => x == null);
		if (_components.Where(x => x.name == "EndGoal" && x.Connected == true).Any())
		{
			EngComponent block = _components.Where(x => x.name == "EndGoal").First();
			if (block.GetType().IsAssignableFrom(typeof(BaseCircuit)))
			{
				BaseCircuit actualBlock = (BaseCircuit)block;
				if (actualBlock.Current >= RequiredPower)
				{
					Destroy(gameObject);
				}
			}

		}
	}
}
