using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BaseCircuit : EngComponent
{
	[SerializeField]
	protected  List<string> _sourceNames;

	protected List<string> SourceNames
	{
		get { return _sourceNames; }
		set { _sourceNames = value; }
	}
	
	protected List<RaycastHit2D> _oldResults;
	[SerializeField]
	protected float _current;
	protected List<BaseCircuit> _pseudoParents = new List<BaseCircuit>();
    protected List<BaseCircuit> _pseudoChildren = new List<BaseCircuit>();
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

	protected virtual void Start()
	{
		if (_sourceNames == null)
		{
			_sourceNames = new List<string>();
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
		else if (_updateResults.Where(x => x.transform.GetComponent<BaseCircuit>().SourceNames.Except(SourceNames).Any() && !_pseudoParents.Contains(x.transform.GetComponent<BaseCircuit>()) && !_pseudoChildren.Contains(x.transform.GetComponent<BaseCircuit>())).Any())
		{
			foreach (BaseCircuit circ in _updateResults.Where(x => x.transform.GetComponent<BaseCircuit>().SourceNames.Except(SourceNames).Any() && !_pseudoParents.Contains(x.transform.GetComponent<BaseCircuit>())).Select(x => x.transform.GetComponent<BaseCircuit>()))
			{
				_pseudoParents.Add(circ);
			}
		}
		if (_pseudoParents.Contains(this))
		{
			_pseudoParents.Remove(this);
		}
		foreach (List<string> source in _pseudoParents.Select(x => x.SourceNames))
		{
			if (!SourceNames.Any() || source.Except(SourceNames).Any())
			{
				foreach (string sourceName in source.Except(SourceNames))
				{
					if (sourceName != gameObject.name)
					{
						SourceNames.Add(sourceName);
					}

				}
			}
		}

		float power = 0;
		foreach (BaseCircuit parentCircuit in _pseudoParents)
		{

			power += parentCircuit.Current;
		}
		Current = power;
	}

	protected void VerifyCurrent(List<RaycastHit2D> rayCollection)
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
