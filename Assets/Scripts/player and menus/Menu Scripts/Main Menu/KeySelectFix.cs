using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class KeySelectFix : MonoBehaviour {

    public EventSystem eventsys;
    public GameObject toSelect;

    // Use this for initialization
    void Start()
    {

    }
    void Update()
    {
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            if (eventsys.currentSelectedGameObject == null)
            {
                eventsys.SetSelectedGameObject(toSelect);
            }
        }
    }
}
