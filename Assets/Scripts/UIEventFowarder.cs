using UnityEngine;

public class UIEventFowarder : MonoBehaviour {

    public delegate void UIEvent(GameObject originalObject);
    public event UIEvent Buttonevent;
    public event DialogueController.NodeEvent NodeEvent;

	public void Event() {
        if (Buttonevent != null) {
            Buttonevent.Invoke(gameObject); 
        }
        if (NodeEvent != null) {
            NodeEvent.Invoke(); 
        }
    }

}
