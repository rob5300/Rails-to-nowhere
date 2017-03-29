using System;
using UnityEngine;
using UnityEngine.UI;

public class InputPad : MonoBehaviour {

    public Text OutputText;
    public string currentlyEnteredCode = "";
    public Door doorToOpen;

    public void Start() {
        OutputText.text = "****";
    }

    public void KeyPress(string key) {
        key = key.ToLower();
        if (currentlyEnteredCode.ToString().ToCharArray().Length >= 4) return;
        currentlyEnteredCode += key;
        OutputText.text = currentlyEnteredCode;
    }

    public void CheckCode() {
        if(Convert.ToInt32(currentlyEnteredCode) == Progression.Code) {
            //Code matches.
            OutputText.color = Color.green;
            doorToOpen.Open();
        }
        else {
            OutputText.color = Color.red;
            Invoke("ResetColour", 0.3f);
            currentlyEnteredCode = "";
        }
    }

    public void ResetColour() {
        OutputText.color = Color.white;
        OutputText.text = "****";
    }
}
