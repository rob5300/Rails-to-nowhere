using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SelectKeyboard : MonoBehaviour {

    public EventSystem eventSystem;
    public GameObject selectedObject;

    private bool buttonSelected;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Vertical") != 0 && buttonSelected == false)
        {
            eventSystem.SetSelectedGameObject(selectedObject);
            buttonSelected = true;
        }

       // if(!selectedObject.parent.enabled)
       // {
           // eventSystem.SetSelectedGameObject(mainmenu.buttons[0]);
       // }
    }

    private void OnDisable()
    {
        buttonSelected = false;
    }
}