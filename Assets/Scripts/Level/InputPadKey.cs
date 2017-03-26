using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class InputPadKey : Entity {

    public string key;
    public InputPad inputpad;

    public void Reset() {
        inputpad = GetComponentInParent<InputPad>();
    }

    public override void OnInteract() {
        if(key != "enter") {
            inputpad.KeyPress(key);
        }
        else {
            inputpad.CheckCode();
        }
        
    }
}
