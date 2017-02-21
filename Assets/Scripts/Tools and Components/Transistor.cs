using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Transistor : BaseCircuit 
{

    public override float Current { get; set; }
                                       // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	protected override void Update () 
    {
        base.Update();
        if (_updateResults.Where(x => x.transform.position.y > transform.position.y && _pseudoParents.Contains(x.transform.GetComponent<BaseCircuit>())).Any())
        {
            if (_pseudoParents.Where(x => x.transform.position.y > transform.position.y).First().Current > 0f)
            {
				List<RaycastHit2D> conns = _updateResults.Where(x => x.transform.position.y < transform.position.y).ToList();
				if (conns.Count > 0)
				{
					BaseCircuit botSideConnection = conns.First().transform.GetComponent<BaseCircuit>();
					Current = botSideConnection.Current;
				}

            }
        }
        else
        {
            Current = 0f;
        }
    }
}
