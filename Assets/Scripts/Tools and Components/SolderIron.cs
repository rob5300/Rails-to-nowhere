using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SolderIron : MonoBehaviour
{
	public GameObject _solder;
	public Camera _puzzleCam;
	private List<EngComponent> _circuitBlocks;
	private List<GameObject> _boardAreas;
	// Use this for initialization
	void Start ()
	{
		_circuitBlocks = GameObject.FindGameObjectWithTag("Board").transform.GetComponentsInChildren<EngComponent>().ToList();
		Transform child = GameObject.FindGameObjectWithTag("Board").transform.GetChild(0);
		_boardAreas = new List<GameObject>();
		foreach (Transform childChild in child)
		{
			_boardAreas.Add(childChild.gameObject);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 mouse = _puzzleCam.ScreenToWorldPoint(Input.mousePosition);
		mouse.z = transform.position.z;
		transform.position = mouse;
		if (Input.GetMouseButtonDown(0))
		{
			PerformSolderCheck(mouse);
		}
	}

	private void PerformSolderCheck(Vector3 mouse)
	{
		GameObject closestObj = _boardAreas.OrderBy(x => Vector2.Distance(mouse, x.transform.position)).First();
		if (Vector2.Distance(closestObj.transform.position, mouse) < 1 && closestObj.tag == "BoardCell")
		{
			GameObject solderBlock = Instantiate(_solder, closestObj.transform.position, closestObj.transform.rotation) as GameObject;
			SpriteRenderer renderer = solderBlock.GetComponent<SpriteRenderer>();
			renderer.sortingOrder = 101;
			solderBlock.transform.position = new Vector3(solderBlock.transform.position.x, closestObj.transform.position.y, 0.1046143f);
			solderBlock.transform.parent = closestObj.transform.parent.parent;
			closestObj.transform.parent.parent.GetComponent<TestCurrentController>()._circuits.Add(solderBlock.GetComponent<Solder>());
			_circuitBlocks.Add(solderBlock.GetComponent<EngComponent>());
		}
	}


}
