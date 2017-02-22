using UnityEngine;
using System.Collections;

public class Resistor : BaseCircuit {


	public override float Current
	{
		get
		{
			return base.Current;
		}
		set
		{
			_current = value / 2;
		}
	}

}
