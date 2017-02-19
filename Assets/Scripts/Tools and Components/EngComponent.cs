using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class EngComponent : MonoBehaviour
{
	public bool _connected = false;

	internal virtual void Update()
	{
		List<RaycastHit2D> result = Physics2D.CircleCastAll(transform.position, 0.180f, Vector2.zero, 0f).Where(x => x.transform.tag == "Circuit").ToList();
		if (result.Where(x => x.transform.GetComponent<EngComponent>()._connected == true).Any())
		{
			_connected = true;
		}
	}
}
