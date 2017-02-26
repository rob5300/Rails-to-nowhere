using UnityEngine;
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
		_blocks.RemoveAll(x => x == null);
		_mouseInWorldSpace = PuzzleCam.ScreenToWorldPoint(Input.mousePosition);
		if (Input.GetKeyDown(KeyCode.E) && !_move && Vector2.Distance(_mouseInWorldSpace, transform.position) < (GetComponent<Renderer>().bounds.size.x / 2) + 0.02f)
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

	protected virtual void RemoveObject(Vector3 position = default(Vector3))
	{
		GameObject closestObj = _blocks.OrderBy(x => Vector2.Distance(position, x.transform.position)).First().gameObject;
		if (Vector2.Distance(closestObj.transform.position, position) < 1 && closestObj.tag == Prefab.tag && closestObj.name.Contains(Prefab.name))
		{
			_blocks.Remove(closestObj.GetComponent<EngComponent>());
			closestObj.transform.parent.GetComponent<TestCurrentController>()._components.Remove(closestObj.GetComponent<EngComponent>());
			Destroy(closestObj);
		}
	}

	protected virtual void PlaceObject(Vector3 position = default(Vector3))
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
