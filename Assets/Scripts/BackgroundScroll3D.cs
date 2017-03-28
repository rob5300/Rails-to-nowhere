using UnityEngine;
using System.Collections;

public class BackgroundScroll3D : MonoBehaviour {

    public float RearX;
    public float FrontX;
    public float MoveAmount;
     
	
	// Update is called once per frame
	void Update () {
        foreach (Transform child in transform)
        {
            child.position += new Vector3(MoveAmount*Time.deltaTime, 0f, 0f);

            if (child.position.x < transform.position.x + FrontX)
            {
               child.position = new Vector3(transform.position.x + RearX, child.position.y, child.position.z);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Debug.DrawRay(new Vector3(transform.position.x + RearX, transform.position.y - 5, transform.position.z), transform.forward * 5, Color.blue);
        Debug.DrawRay(new Vector3(transform.position.x + FrontX, transform.position.y - 5, transform.position.z), transform.forward * 5, Color.red);
    }
}
