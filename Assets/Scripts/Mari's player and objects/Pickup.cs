using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {


    public int distanceToItem;

    void Update()
    {

        Collect();

    }

    void Collect()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {

            RaycastHit hit;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);



            if (Physics.Raycast(ray, out hit, distanceToItem))
            {
                if (hit.collider.gameObject.tag == "pickup")
                {

                    Destroy(hit.collider.gameObject);



                }
            }
        }
    }
}
