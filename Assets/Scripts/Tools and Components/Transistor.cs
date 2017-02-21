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
            if (_pseudoParents.Where(x => x.transform.position.y > transform.position.y).First().GetComponent<BaseCircuit>().Current > 0f)
            {
                BaseCircuit botSideConnection = _updateResults.Where(x => x.transform.position.y < transform.position.y).First().transform.GetComponent<BaseCircuit>();
                print(botSideConnection.Current);
                Current = botSideConnection.Current;
            }
        }
        else
        {
            Current = 0f;
        }
    }
}
