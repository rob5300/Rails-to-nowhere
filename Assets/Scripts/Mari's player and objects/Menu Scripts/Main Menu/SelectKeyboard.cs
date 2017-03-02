using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class SelectKeyboard : MonoBehaviour {

    public EventSystem eventSystem;
    public GameObject onenableob;
    public GameObject ondisableob;

    public void OnEnable()
    {
            eventSystem.SetSelectedGameObject(onenableob);
    }

    public void OnDisable() {
        eventSystem.SetSelectedGameObject(ondisableob);
    }
}