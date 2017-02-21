using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BaseCircuit : EngComponent
{
	protected List<RaycastHit2D> _oldResults;
	[SerializeField]
	protected float _current;
	private List<BaseCircuit> _pseudoParents = new List<BaseCircuit>();
	private List<BaseCircuit> _pseudoChildren = new List<BaseCircuit>();
	public virtual float Current
	{
		get
		{
			return _current;
		}
		set
		{
			_current = value;
			if (_current > 5)
			{
				Destroy(gameObject);
			}
		}
	}

	public List<BaseCircuit> PseudoParent
	{
		get
		{
			return _pseudoParents;
		}
	}

	public List<BaseCircuit> PseudoChildren
	{
		get
		{
			return _pseudoChildren;
		}
	}

	protected override void Update()
	{
		base.Update();
		if (_oldResults != null)
		{
			VerifyCurrent(_updateResults);
		}
		_oldResults = _updateResults;
		if (_pseudoParents.Count == 0)
		{
			if (_updateResults.Where(x => x.transform.GetComponent<BaseCircuit>().Current > 0).Any())
			{
				foreach (BaseCircuit circuitInstance in _updateResults.Select(x => x.transform.GetComponent<BaseCircuit>()).Where(x => x.Current > 0))
				{
					_pseudoParents.Add(circuitInstance);
				}
			}
		}
		if (_pseudoParents.Contains(this))
		{
			_pseudoParents.Remove(this);
		}
		float power = 0;
		foreach (BaseCircuit parentCircuit in _pseudoParents)
		{

			power += parentCircuit.Current;
		}
		Current = power;
	}

	private void VerifyCurrent(List<RaycastHit2D> rayCollection)
	{
		foreach (RaycastHit2D result in rayCollection)
		{
			if (!_oldResults.Contains(result))
			{
				BaseCircuit component = result.transform.gameObject.GetComponent<BaseCircuit>();
				if (component.Current == 0 && !_pseudoChildren.Contains(component))
				{
					_pseudoChildren.Add(component);
				}
				else if (!_pseudoParents.Contains(component))
				{
					_pseudoParents.Add(component);
				}
			}
		}
	}

	protected virtual void OnDestroy()
	{
		foreach (RaycastHit2D rayResult in _updateResults)
		{
			foreach (BaseCircuit pseudoParent in _pseudoParents)
			{
				pseudoParent._pseudoChildren.Remove(this);
			}
		}
	}
}
