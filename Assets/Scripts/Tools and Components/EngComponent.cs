using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class EngComponent : MonoBehaviour
{
	protected List<RaycastHit2D> _updateResults;
	[SerializeField]
	protected bool _connected = false;


	/// <summary>
	/// Checks if the component is connected to a valid path to the starting point.
	/// </summary>
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

	/// <summary>
	/// calls CheckConnections every frame. Ensure you call base.Update whenever you are inheriting from EngComponent so that the check is called.
	/// </summary>
	protected virtual void Update()
	{
		_updateResults = CheckConnections();
		if (_updateResults.Where(x => x.transform.GetComponent<EngComponent>().Connected == true).Any())
		{
			Connected = true;
		}
	}

	/// <summary>
	/// Base implementation uses a Physics2D.CircleCastAll to an appropriate size based on the X-size of the Renderer. Every other class relies on this, do not override unless completely essential.
	/// </summary>
	/// <returns></returns>
	protected virtual List<RaycastHit2D> CheckConnections()
	{
		return Physics2D.CircleCastAll(transform.position, (GetComponent<Renderer>().bounds.size.x / 2) + 0.01f, Vector2.zero, 0f).Where(x => x.transform.tag == "Circuit").ToList();
	}
}
