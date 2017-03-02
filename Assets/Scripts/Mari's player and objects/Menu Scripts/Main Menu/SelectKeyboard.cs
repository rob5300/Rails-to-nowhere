using UnityEngine;
using UnityEngine.EventSystems;

public class SelectKeyboard : MonoBehaviour {

    public EventSystem eventSystem;
    public GameObject onenableob;
    public GameObject ondisableob;

    public void OnEnable() {
        eventSystem.SetSelectedGameObject(onenableob);
    }

    private void OnDisable() {
        eventSystem.SetSelectedGameObject(ondisableob);
    }
}