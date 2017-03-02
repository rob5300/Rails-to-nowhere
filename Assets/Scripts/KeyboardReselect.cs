using UnityEngine;
using UnityEngine.EventSystems;

public class KeyboardReselect : MonoBehaviour {

    public EventSystem eventsys;
    public GameObject toSelect;
	
	void Update () {
	    if(Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) {
            if(eventsys.currentSelectedGameObject == null) {
                eventsys.SetSelectedGameObject(toSelect);
            }
        }
	}
}
