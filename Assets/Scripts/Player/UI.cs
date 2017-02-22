using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI : MonoBehaviour {

    public static UI ui;

    public Text debugText;
    public DialogueUI dialogueUI = new DialogueUI();
    //public int TextBufferSize = 30;

    List<Button> _responseButtons = new List<Button>();
    //List<string> _textBuffer = new List<string>();

    void Start() {
        //Assign the static variable player to the only instance of this class that should exist.
        if (!ui) ui = this;
        else Debug.LogError("Multiple UI instances on objects: " + ui.name + ", " + name);

        //Disable objects incase they are left enabled;
        dialogueUI.Parent.SetActive(false);
    }

    public static void StartDialogueConversation(DialogueNode startingNode) {
        
    }
}

[System.Serializable]
public struct DialogueUI {
    public GameObject Parent;
    public InputField MainTextArea;
    public GameObject ResponseButtonArea;
    public GameObject ResponseButton;
}
