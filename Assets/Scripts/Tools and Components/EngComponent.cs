using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class EngComponent : MonoBehaviour
{
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
		List<RaycastHit2D> result = Physics2D.CircleCastAll(transform.position, 0.180f, Vector2.zero, 0f).Where(x => x.transform.tag == "Circuit").ToList();
		if (result.Where(x => x.transform.GetComponent<EngComponent>().Connected == true).Any())
		{
			Connected = true;
		}
	}
}
