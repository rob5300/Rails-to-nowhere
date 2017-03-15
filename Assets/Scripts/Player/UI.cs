using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class UI : MonoBehaviour {

    public static UI ui;
    public static bool MenuOpen = false;
    public static DialogueUI dialogueUI;
    public static InventoryUI inventoryUI;
    public static bool inventoryOpen = false;
    public static bool puzzle2DOpen = false;
    public static GameObject puzzle2D;
    public static Text debugText;

    public Text debugTextObject;
    public Text HoverName;
    public Text HoverDescription;
    public GameObject CrosshairOb;
    public DialogueUI dialogueUIObjects = new DialogueUI();
    public InventoryUI inventoryUIObjects = new InventoryUI();

    public static List<GameObject> responseButtons = new List<GameObject>();
    public static List<GameObject> inventoryItems = new List<GameObject>();
    public static GameObject Crosshair;

    void Awake() {
        //Assign the static variable player to the only instance of this class that should exist.
        if (!ui) ui = this;
        else {
            Debug.LogError("Multiple UI instances on objects: " + ui.name + ", " + name);
            //We HAVE to return to stop execution if there is another instance that exists already, otherwise we may override the original static variables.
            return;
        }

        dialogueUI = dialogueUIObjects;
        inventoryUI = inventoryUIObjects;
        debugText = debugTextObject;
        Crosshair = CrosshairOb;

        //Disable UI objects incase they are left enabled.
        dialogueUI.ResponseButton.SetActive(false);
        dialogueUIObjects.Parent.SetActive(false);

        inventoryUI.Parent.SetActive(false);
        inventoryUI.BaseInventoryItem.SetActive(false);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.I)) {
            if (!inventoryOpen) {
                inventoryUI.Parent.SetActive(true);
                inventoryOpen = true;

                CleanInventoryUI();
                UpdateInventoryUI();

                LockPlayerController();
                UnlockCursor();
            }
            else {
                inventoryUI.Parent.SetActive(false);
                inventoryOpen = false;
                LockCursor();
                UnlockPlayerController();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (puzzle2DOpen) {
                Hide2DPuzzle();
            }
        }
    }

    //Dialogue Code
    #region
    public static void DialogueConversation(DialogueNode node) {
        MenuOpen = true;
        dialogueUI.MainTextArea.text = node.Text;
        //Currently there is no handeling for if the text overlaps the box. In future there will be handeling for cycling through the same text contents using a buffer.
        ShowResponses(node);

        dialogueUI.Parent.SetActive(true);
    }

    public static void ShowResponses(DialogueNode currentNode) {
        CleanupSpeechUI();
        GameObject button;
        foreach (DialogueNode response in DialogueController.GetNodeResponses(currentNode)) {
            button = (GameObject)Instantiate(dialogueUI.ResponseButton, dialogueUI.ResponseButtonArea.transform);
            responseButtons.Add(button);
            button.GetComponent<Button>().GetComponentInChildren<Text>().text = response.ResponseText;
            button.name = response.Key;
            button.SetActive(true);
        }

        UnlockCursor();
        LockPlayerController();
    }

    public void ExitSpeechUI() {
        dialogueUI.Parent.SetActive(false);
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

    public static void CleanupSpeechUI() {
        if (responseButtons.Count > 0) {
            for (int i = responseButtons.Count; i > 0; i--) {
                Destroy(responseButtons[i-1]);
            } 
        }
    }
    #endregion

    public static void UnlockCursor() {
        Cursor.lockState = CursorLockMode.None;
        Crosshair.SetActive(false);
        Cursor.visible = true;
    }

    public static void UnlockPlayerController() {
        Player.player.Controller.enabled = true;
        Player.player.blurEffect.enabled = false;
    }

    public static void LockPlayerController() {
        Player.player.Controller.enabled = false;
        Player.player.blurEffect.enabled = true;
    }

    public static void LockCursor() {
        Cursor.lockState = CursorLockMode.Locked;
        Crosshair.SetActive(true);
        Cursor.visible = false;
    }

    //Inventory Code
    #region
    public static void UpdateInventoryUI() {
        GameObject newInvItem;
        UIItemSlot newSlot;

        List<Inventory.ItemSlot> popItemSlots = Player.player.inventory.GetPopulatedItemSlots();
        if (popItemSlots.Count < 0) return;
        foreach (Inventory.ItemSlot itemSlot in popItemSlots) {
            newInvItem = (GameObject)Instantiate(inventoryUI.BaseInventoryItem, inventoryUI.InventoryItemArea.transform);
            inventoryItems.Add(newInvItem);
            newSlot = newInvItem.GetComponent<UIItemSlot>();
            Item item = Item.GetItem(itemSlot.ItemID);
            newSlot.ItemSprite.sprite = item.InventorySprite;
            newSlot.ItemName.text = item.Name;
            newSlot.ItemType.text = item.GetType().ToString();
            if(itemSlot.ItemQuantity < 10) {
                newSlot.ItemQuantity.text = "x0" + itemSlot.ItemQuantity;
            }
            else {
                newSlot.ItemQuantity.text = "x" + itemSlot.ItemQuantity;
            }
            newInvItem.SetActive(true);
        }
    }

    public static void CleanInventoryUI() {
        if (inventoryItems.Count > 0) {
            for (int i = inventoryItems.Count; i > 0; i--) {
                Destroy(inventoryItems[i - 1]);
            }
        }
    }

    public static void InventoryItemClickEvent(UIItemSlot slot){

    }
    #endregion

    //Puzzle 2D Code
    #region
    public static void Show2DPuzzle(GameObject puzzleObject) {
        if (!puzzle2DOpen) {
            LockPlayerController();
            UnlockCursor();
            puzzle2D = puzzleObject;
            puzzle2D.SetActive(true);
            puzzle2DOpen = true;
        }
    }

    public static void Hide2DPuzzle() {
        if (puzzle2DOpen) {
            LockCursor();
            UnlockPlayerController();
            puzzle2D.SetActive(false);
        }
    }
    #endregion
}

[System.Serializable]
public struct DialogueUI {
    public GameObject Parent;
    public InputField MainTextArea;
    public GameObject ResponseButtonArea;
    public GameObject ResponseButton;
}

[System.Serializable]
public struct InventoryUI
{
    public GameObject Parent;
    public GameObject BaseInventoryItem;
    public GameObject InventoryItemArea;

}
