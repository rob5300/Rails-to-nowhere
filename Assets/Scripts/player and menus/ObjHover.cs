using UnityEngine;
using System.Collections;

public class ObjHover : MonoBehaviour {
    public string collidedmesh;

    bool isHovering;
    
    void Start()
    {
        collidedmesh = transform.name;
    }
    void OnMouseEnter()
    {
        isHovering = true;
    }

    void OnMouseExit()
    {
        isHovering = false;
    }

    void OnGUI()
    {
        if (isHovering == true)
        {
            GUI.Box(new Rect(300, 100, 100, 20), "Pick me up!");
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
