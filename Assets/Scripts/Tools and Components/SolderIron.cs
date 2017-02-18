using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SolderIron : MonoBehaviour
{
	public GameObject _solder;
	public Camera _puzzleCam;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 mouse = _puzzleCam.ScreenToWorldPoint(Input.mousePosition);
		mouse.z = transform.position.z;
		transform.position = mouse;
		if (Input.GetMouseButton(0))
		{
			GetComponent<BoxCollider2D>().enabled = true;

		}
		else
		{
			GetComponent<BoxCollider2D>().enabled = false;
		}
	
	}

	void OnTriggerStay2D(Collider2D coll)
	{
		if (coll.transform.position.y > transform.position.y && coll.gameObject.tag == "Circuit")
		{
			List<GameObject> children = new List<GameObject>();
			foreach (Transform child in coll.transform.parent.transform)
			{
				if (child.tag == "Circuit")
				{
					children.Add(child.gameObject);
				}
			}
			if (children.Where(x => x.GetComponent<BoxCollider2D>().IsTouching(coll)).Count() < 2)
			{
				Instantiate(_solder, coll.bounds.min - coll.bounds.center, coll.transform.rotation);
			}
		}
	}
}
