using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ObjectHoverFinal : MonoBehaviour
{


    Vector3 raycastStart;
    Vector3 fowardDirection;
    public float ReachDistance;
    public Text objectInfo;

    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {
        raycastStart = Camera.main.transform.position;
       
        fowardDirection = Camera.main.transform.TransformDirection(Vector3.forward);

        Debug.DrawRay(raycastStart, fowardDirection * ReachDistance, Color.magenta);

        if(Physics.Raycast(raycastStart, fowardDirection, ReachDistance))
        {
          // Debug.Log("Pick me up!");
          objectInfo.enabled = true;
        }
        else
        {
            objectInfo.enabled = false;
        }
    }
}    
    
