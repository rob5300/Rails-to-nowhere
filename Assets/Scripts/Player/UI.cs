using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class UI : MonoBehaviour {

    public static UI ui;
    public static bool MenuOpen = false;
    public static DialogueUI dialogue;
    public static Text debugText;

    public Text debugTextObject;
    public DialogueUI dialogueUIObjects = new DialogueUI();

    public static List<GameObject> responseButtons = new List<GameObject>();

    void Start() {
        //Assign the static variable player to the only instance of this class that should exist.
        if (!ui) ui = this;
        else {
            Debug.LogError("Multiple UI instances on objects: " + ui.name + ", " + name);
            //We HAVE to return to stop execution if there is another instance that exists already, otherwise we may override the original static variables.
            return;
        }

        dialogue = dialogueUIObjects;
        debugText = debugTextObject;
        if (dialogue.ResponseButton.activeSelf) dialogue.ResponseButton.SetActive(false);
        //Disable objects incase they are left enabled;
        dialogueUIObjects.Parent.SetActive(false);
    }

    public static void DialogueConversation(DialogueNode node) {
        MenuOpen = true;
        dialogue.MainTextArea.text = node.Text;
        //Currently there is no handeling for if the text overlaps the box. In future there will be handeling for cycling through the same text contents using a buffer.
        ShowResponses(node);

        dialogue.Parent.SetActive(true);
    }

    public static void ShowResponses(DialogueNode currentNode) {
        CleanupUI();
        GameObject button;
        foreach (DialogueNode response in DialogueController.GetNodeResponses(currentNode)) {
            button = (GameObject)Instantiate(dialogue.ResponseButton, dialogue.ResponseButtonArea.transform);
            responseButtons.Add(button);
            button.GetComponent<Button>().GetComponentInChildren<Text>().text = response.ResponseText;
            button.name = response.Key;
            button.SetActive(true);
        }

        UnlockCursor();
        LockPlayerController();
    }

    public void ExitSpeechUI() {
        dialogue.Parent.SetActive(false);
        MenuOpen = false;
        LockCursor();
        UnlockPlayerController();
    }

    public void ProgressDialogue() {
        //To advance the dialogue if there is too much to display or its split.
    }

    public static void HandleDialogueResponse(string nodekey) {
        DialogueConversation(DialogueController.GetNode(nodekey));
    }

    public static void CleanupUI() {
        if (responseButtons.Count > 0) {
            for (int i = responseButtons.Count; i > 0; i--) {
                Destroy(responseButtons[i-1]);
            } 
        }
    }

    public static void UnlockCursor() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public static void UnlockPlayerController() {
        Player.player.Controller.enabled = true;
    }

    public static void LockPlayerController() {
        Player.player.Controller.enabled = false;
    }

    public static void LockCursor() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}

[System.Serializable]
public struct DialogueUI {
    public GameObject Parent;
    public InputField MainTextArea;
    public GameObject ResponseButtonArea;
    public GameObject ResponseButton;
}
