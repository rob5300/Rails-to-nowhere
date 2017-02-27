﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class BaseTool : MonoBehaviour {
	[SerializeField]
	protected GameObject _prefab;
	[SerializeField]
	protected Camera _puzzleCam;
	protected List<EngComponent> _blocks;
	protected List<GameObject> _boardAreas;
	protected bool _move = false;
	protected Vector3 _mouseInWorldSpace; //this is in case a method is overidden that needs this value

	/// <summary>
	/// The prefab to be spawned by the BaseTool instance.
	/// </summary>
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


	/// <summary>
	/// The target camera to use.
	/// </summary>
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

	/// <summary>
	/// Base implementation checks the puzzle instance in the world and loads it into the BaseTool's private fields. You may want to consider overriding this if there is multiple puzzles in the world.
	/// </summary>
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

	/// <summary>
	/// Base implementation removes empty blocks within a puzzle, gets the mouse in world and assigns it to _mouseInWorldSpace, then checks if the player has picked up the tool by calling CheckMove(), then based on CheckMove() changing the value of _move to true or false, it will then follow the mouse cursor and check the Place/Remove calls.
	/// </summary>
	protected virtual void Update()
	{
		_blocks.RemoveAll(x => x == null);
		_mouseInWorldSpace = PuzzleCam.ScreenToWorldPoint(Input.mousePosition);
		CheckMove();
		if (_move)
		{
			_mouseInWorldSpace.z = transform.position.z;
			transform.position = _mouseInWorldSpace;
			CheckPlaceRemoveCalls();
		}
	}

	/// <summary>
	/// Base implementation checks if the player is trying to place or remove a component by checking MB1 and MB2 respectively.
	/// </summary>
	protected virtual void CheckPlaceRemoveCalls()
	{
		if (Input.GetMouseButtonDown(0))
		{
			PlaceObject(_mouseInWorldSpace);
		}
		if (Input.GetMouseButtonDown(1))
		{
			RemoveObject(_mouseInWorldSpace);
		}
	}


	/// <summary>
	/// Base implementation uses a GetKeyDown call on the E key to run the code required to pick up a tool within 2D space. You may want to override this if it's 3D.
	/// </summary>
	protected virtual void CheckMove()
	{
		if (Input.GetKeyDown(KeyCode.E) && !_move && Vector2.Distance(_mouseInWorldSpace, transform.position) < (GetComponent<Renderer>().bounds.size.x / 2) + 0.02f)
		{
			_move = true;
		}
		else if (Input.GetKeyDown(KeyCode.E) && _move)
		{
			_move = false;
		}
	}

	/// <summary>
	/// Base implementation uses positions to remove 1 object that has an EngComponent object instances and is of the prefab type.
	/// </summary>
	/// <param name="position">Position is traditionally the mouse in world position. See base.Update().</param>
	protected virtual void RemoveObject(Vector3 position)
	{
		GameObject closestObj = _blocks.OrderBy(x => Vector2.Distance(position, x.transform.position)).First().gameObject;
		if (Vector2.Distance(closestObj.transform.position, position) < 1 && closestObj.tag == Prefab.tag && closestObj.name.Contains(Prefab.name))
		{
			_blocks.Remove(closestObj.GetComponent<EngComponent>());
			closestObj.transform.parent.GetComponent<TestCurrentController>()._components.Remove(closestObj.GetComponent<EngComponent>());
			Destroy(closestObj);
		}
	}

	/// <summary>
	/// Base implementation uses positions to add 1 object of the prefab type.
	/// </summary>
	/// <param name="position">Position is traditionally the mouse in world position. See base.Update().</param>
	protected virtual void PlaceObject(Vector3 position)
	{
		GameObject closestBoardObj = _boardAreas.OrderBy(x => Vector2.Distance(position, x.transform.position)).First();
		GameObject closestCircuitObj = _blocks.OrderBy(x => Vector2.Distance(position, x.transform.position)).First().gameObject;
		if (Vector2.Distance(closestBoardObj.transform.position, position) < 1 && Vector2.Distance(closestBoardObj.transform.position, closestCircuitObj.transform.position) > 0.1)
		{
			GameObject block = Instantiate(Prefab, closestBoardObj.transform.position, closestBoardObj.transform.rotation) as GameObject;
			SpriteRenderer renderer = block.GetComponent<SpriteRenderer>();
			renderer.sortingOrder = 101;
			block.transform.position = new Vector3(block.transform.position.x, closestBoardObj.transform.position.y, 0.1046143f);
			block.transform.parent = closestBoardObj.transform.parent.parent;
			closestBoardObj.transform.parent.parent.GetComponent<TestCurrentController>()._components.Add(block.GetComponent<BaseCircuit>());
			_blocks.Add(block.GetComponent<EngComponent>());
		}
	}
}
