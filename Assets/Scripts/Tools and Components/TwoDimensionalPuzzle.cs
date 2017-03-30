using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TwoDimensionalPuzzle : Puzzle {

	public List<EngComponent> _components;
	[SerializeField]
	private float _requiredPower;

	public float RequiredPower
	{
		get { return _requiredPower; }
		set { _requiredPower = value; }
	}

	void Start()
	{
		_components = GetComponentsInChildren<EngComponent>().ToList();
		TwoDimensionalUIController.UI.gameObject.SetActive(true);
        TwoDimensionalUIController.UI.FindComponents();
    }

	void Update()
	{
		_components.RemoveAll(x => x == null);
		if (_components.Where(x => x.name == "EndGoal" && x.Connected == true).Any())
		{
			if (_components.Where(x => x.name.ToUpper().Contains("START")).Count() > 1)
			{
				if (_components.Where(x => x.name.ToUpper().Contains("SISTOR") && x.Connected == true).Count() == 0)
				{
					return;
				}
			}
			EngComponent block = _components.Where(x => x.name == "EndGoal").First();
			if (block.GetType().IsAssignableFrom(typeof(BaseCircuit)))
			{
				BaseCircuit actualBlock = (BaseCircuit)block;
				if (actualBlock.Current >= RequiredPower)
				{
					PuzzleDone(CarriagePuzzleController.PuzzleType.T2D);
                    TwoDimensionalUIController.UI.ValidateInventory();
                    UI.Hide2DPuzzle();
				}
			}

		}
	}

}
