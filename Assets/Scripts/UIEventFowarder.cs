using UnityEngine;

public class UIEventFowarder : MonoBehaviour {

    //Currently only for the speech ui, later can be made to foward anywhere.
	public void Event() {
        UI.HandleDialogueResponse(gameObject.name);
    }

}
