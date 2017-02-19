﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BaseTool : MonoBehaviour {
	[SerializeField]
	protected GameObject _prefab;
	[SerializeField]
	protected Camera _puzzleCam;
	protected List<EngComponent> _blocks;
	protected List<GameObject> _boardAreas;
	protected bool _move = false;
	protected Vector3 _mouseInWorldSpace; //this is in case a method is overidden that needs this value

	public GameObject Prefab
	{
		get
		{
			return _prefab;
		}

		set
		{
			_prefab = value;
		}
	}

	public Camera PuzzleCam
	{
		get
		{
			return _puzzleCam;
		}

		set
		{
			_puzzleCam = value;
		}
	}

	// Use this for initialization
	protected virtual void Start()
	{
		_blocks = GameObject.FindGameObjectWithTag("Board").transform.GetComponentsInChildren<EngComponent>().ToList();
		Transform child = GameObject.FindGameObjectWithTag("Board").transform.GetChild(0);
		_boardAreas = new List<GameObject>();
		foreach (Transform childChild in child)
		{
			_boardAreas.Add(childChild.gameObject);
		}
	}

	// Update is called once per frame
	protected virtual void Update()
	{
		_mouseInWorldSpace = PuzzleCam.ScreenToWorldPoint(Input.mousePosition);
		if (Input.GetKeyDown(KeyCode.E) && !_move && Vector2.Distance(_mouseInWorldSpace, transform.position) < 1)
		{
			_move = true;
		}
		else if (Input.GetKeyDown(KeyCode.E) && _move)
		{
			_move = false;
		}
		if (_move)
		{

			_mouseInWorldSpace.z = transform.position.z;
			transform.position = _mouseInWorldSpace;
			if (Input.GetMouseButtonDown(0))
			{
				PlaceObject(_mouseInWorldSpace);
			}
			if (Input.GetMouseButtonDown(1))
			{
				RemoveObject(_mouseInWorldSpace);
			}
		}

	}

	protected virtual void RemoveObject(Vector3 position)
	{
		GameObject closestObj = _blocks.OrderBy(x => Vector2.Distance(position, x.transform.position)).First().gameObject;
		if (Vector2.Distance(closestObj.transform.position, position) < 1 && closestObj.tag == Prefab.tag)
		{
			_blocks.Remove(closestObj.GetComponent<EngComponent>());
			closestObj.transform.parent.GetComponent<TestCurrentController>()._circuits.Remove(closestObj.GetComponent<EngComponent>());
			Destroy(closestObj);
		}
	}

	protected virtual void PlaceObject(Vector3 position)
	{
		GameObject closestObj = _boardAreas.OrderBy(x => Vector2.Distance(position, x.transform.position)).First();
		if (Vector2.Distance(closestObj.transform.position, position) < 1 && closestObj.tag == "BoardCell")
		{
			GameObject block = Instantiate(Prefab, closestObj.transform.position, closestObj.transform.rotation) as GameObject;
			SpriteRenderer renderer = block.GetComponent<SpriteRenderer>();
			renderer.sortingOrder = 101;
			block.transform.position = new Vector3(block.transform.position.x, closestObj.transform.position.y, 0.1046143f);
			block.transform.parent = closestObj.transform.parent.parent;
			closestObj.transform.parent.parent.GetComponent<TestCurrentController>()._circuits.Add(block.GetComponent<BaseCircuit>());
			_blocks.Add(block.GetComponent<EngComponent>());
		}
	}
}
