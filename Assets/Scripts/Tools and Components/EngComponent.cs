using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class EngComponent : MonoBehaviour
{
	protected List<RaycastHit2D> _updateResults;
	[SerializeField]
	protected bool _connected = false;

	public bool Connected
	{
		get
		{
			return _connected;
		}

		set
		{
			_connected = value;
		}
	}

	protected virtual void Update()
	{
		_updateResults = CheckConnections();
		if (_updateResults.Where(x => x.transform.GetComponent<EngComponent>().Connected == true).Any())
		{
			Connected = true;
		}
	}

	protected List<RaycastHit2D> CheckConnections()
	{
		return Physics2D.CircleCastAll(transform.position, 0.180f, Vector2.zero, 0f).Where(x => x.transform.tag == "Circuit").ToList();
	}
}
