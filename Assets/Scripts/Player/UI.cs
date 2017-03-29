using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;
using System;

public class UI : MonoBehaviour {

    public static UI ui;
    public static bool MenuOpen = false;
    public static DialogueUI dialogueUI;
    public static InventoryUI inventoryUI;

    public static bool inventoryOpen = false;
    public static bool puzzle2DOpen = false;
    public static bool dialogueUIOpen = false;
    public static bool allowExit = true;
    public static GameObject puzzle2D;
    public static Animator MessageAnimator;
    public static Text MessageText;
    public static Text ThoughtsText;
    public static GameObject ThoughtsObject;
    public static GameObject ThoughtContButton;
    public static GameObject ThoughtExitButton;
    public static List<string> thoughtTextSequence;
    public static List<string> innerDialogueTextSequence;
    public static string PostCutSceneDialogueNodeKey;

    public static Image CutsceneImage;
    public static GameObject CutsceneObject;
    public static GameObject CutsceneContButton;
    public static GameObject CutsceneExitButton;
    public static List<Sprite> CutsceneSprites;

    public static Text InnerDialogueText;
    public static Text InnerDialogueName;
    public static GameObject InnerDialogueObject;
    public static GameObject InnerDialogueContButton;
    public static GameObject InnerDialogueExitButton;

    public static int DialogueMemoryCount = 0;
    public static int DialogueMemoryTotal = 0;
    public static string DialogueMemoryID = "";
    public static NPC DialogueNPC;

    public static Image EndingImage;
    public static Text EndingText;

    public Animator _MessageAnimator;
    public Text _MessageText;
    public Text HoverName;
    public Text HoverDescription;
    public GameObject CrosshairOb;

    public Text _ThoughtsText;
    public GameObject _ThoughtsObject;
    public GameObject _ThoughtContButton;
    public GameObject _ThoughtExitButton;

    public Image _CutsceneImage;
    public GameObject _CutsceneObject;
    public GameObject _CutsceneContButton;
    public GameObject _CutsceneExitButton;

    public Text _InnerDialogueText;
    public Text _InnerDialogueName;
    public GameObject _InnerDialogueObject;
    public GameObject _InnerDialogueContButton;
    public GameObject _InnerDialogueExitButton;

    public Image _EndingImage;
    public Text _EndingText;

    public DialogueUI dialogueUIObjects = new DialogueUI();
    public InventoryUI inventoryUIObjects = new InventoryUI();

    public static List<GameObject> responseButtons = new List<GameObject>();
    public static List<GameObject> inventoryItems = new List<GameObject>();
    public static GameObject Crosshair;
    public static Item selectedItem;

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
        Crosshair = CrosshairOb;

        MessageAnimator = _MessageAnimator;
        MessageText = _MessageText;

        ThoughtsText = _ThoughtsText;
        ThoughtsObject = _ThoughtsObject;
        ThoughtContButton = _ThoughtContButton;
        ThoughtExitButton = _ThoughtExitButton;

        CutsceneObject = _CutsceneObject;
        CutsceneImage = _CutsceneImage;
        CutsceneContButton = _CutsceneContButton;
        CutsceneExitButton = _CutsceneExitButton;

        InnerDialogueText = _InnerDialogueText;
        InnerDialogueObject = _InnerDialogueObject;
        InnerDialogueContButton = _InnerDialogueContButton;
        InnerDialogueExitButton = _InnerDialogueExitButton;
        InnerDialogueName = _InnerDialogueName;

        //Disable UI objects incase they are left enabled.
        dialogueUI.ResponseButton.SetActive(false);
        dialogueUIObjects.Parent.SetActive(false);

        inventoryUI.Parent.SetActive(false);
        inventoryUI.BaseInventoryItem.SetActive(false);
        inventoryUI.ItemInformationPanel.SetActive(false);
        inventoryUI.MemoryParent.SetActive(false);

        CutsceneObject.SetActive(false);
        InnerDialogueObject.SetActive(false);

        EndingImage = _EndingImage;
        EndingText = _EndingText;

        //Load Dialogue Nodes
        DialogueController.LoadDictionary();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.I)) {
            if (!inventoryOpen) {
                inventoryUI.Parent.SetActive(true);
                inventoryOpen = true;
                MenuOpen = true;
                allowExit = true;

                CleanInventoryUI();
                UpdateInventoryUI();

                LockPlayerController();
                UnlockCursor();
            }
            else {
                inventoryUI.MemoryParent.SetActive(false);
                inventoryUI.Parent.SetActive(false);
                inventoryOpen = false;
                MenuOpen = false;
                LockCursor();
                UnlockPlayerController();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (puzzle2DOpen) {
                Hide2DPuzzle();
            }
            if (dialogueUIOpen && allowExit) {
                ExitSpeechUI();
            }
            if (inventoryOpen) {
                inventoryUI.MemoryParent.SetActive(false);
                inventoryUI.Parent.SetActive(false);
                inventoryOpen = false;
                MenuOpen = false;
                LockCursor();
                UnlockPlayerController();
            }
        }
    }

    //Message and thoughts code
    #region
    public static void ShowMessage(string message) {
        MessageText.text = message;
        MessageAnimator.SetTrigger("Play");
    }

    public static void ThoughtSequence(List<string> thoughttext) {
        MenuOpen = true;
        thoughtTextSequence = thoughttext;
        ThoughtsText.text = "\"" + thoughtTextSequence[0] + "\"";
        thoughtTextSequence.RemoveAt(0);
        if(thoughtTextSequence.Count > 0) {
            ThoughtContButton.SetActive(true);
            ThoughtExitButton.SetActive(false);
        }
        else {
            ThoughtContButton.SetActive(false);
            ThoughtExitButton.SetActive(true);
        }
        ThoughtsObject.SetActive(true);
        LockPlayerController();
        UnlockCursor();
    }

    public void ContinueThoughtThoughtText() {
        ThoughtsText.text = "\"" + thoughtTextSequence[0] + "\"";
        thoughtTextSequence.RemoveAt(0);
        if (thoughtTextSequence.Count > 0) {
            ThoughtContButton.SetActive(true);
            ThoughtExitButton.SetActive(false);
        }
        else {
            ThoughtContButton.SetActive(false);
            ThoughtExitButton.SetActive(true);
        }
    }

    public void CloseThought() {
        ThoughtsObject.SetActive(false);
        MenuOpen = false;
        UnlockPlayerController();
        LockCursor();
    }
    #endregion

    //Dialogue Code
    #region
    public static void NewDialogueConversation(NPC npc) {
        DialogueNPC = npc;
        DialogueMemoryID = npc.MemoryItemKey;
        DialogueMemoryTotal = npc.MemoryResponseTotal;
        DialogueMemoryCount = 0;
        dialogueUIOpen = true;
        MenuOpen = true;
        allowExit = true;
        DialogueNode node = DialogueController.GetNode(npc.InitialDialogueNodeKey);
        dialogueUI.MainTextArea.text = node.Text;
        //Currently there is no handeling for if the text overlaps the box. In future there will be handeling for cycling through the same text contents using a buffer.
        ShowResponses(node);

        dialogueUI.Parent.SetActive(true);
    }

    public static void NewDialogueConversation(DialogueNode node, bool canExit) {
        allowExit = canExit;
        if (!allowExit) {
            dialogueUI.ExitButton.SetActive(false);
        }
        //Set NPC and memory id to null to avoid issues and to prevent node checks.
        DialogueNPC = null;
        DialogueMemoryID = null;
        //Set the memory total to -1 to show that no memory should ever be awarded.
        DialogueMemoryTotal = -1;
        DialogueMemoryCount = 0;
        dialogueUIOpen = true;
        MenuOpen = true;
        dialogueUI.MainTextArea.text = node.Text;
        //Currently there is no handeling for if the text overlaps the box. In future there will be handeling for cycling through the same text contents using a buffer.
        ShowResponses(node);

        dialogueUI.Parent.SetActive(true);
    }

    public static void ContinueDialogueConversation(DialogueNode node) {
        dialogueUI.MainTextArea.text = node.Text;
        //Currently there is no handeling for if the text overlaps the box. In future there will be handeling for cycling through the same text contents using a buffer.
        ShowResponses(node);

        SpecialNodeCheck(node);
    }

    //Checks if a node corrisponds to a special node on the npc. If it does then invoke that corrisponding event on the npc.
    private static void SpecialNodeCheck(DialogueNode node) {
        //Return if this dialogue sequence is not from an NPC.
        if (DialogueNPC == null) return;

        if(DialogueNPC is StoryNPC) {
            if(((StoryNPC)DialogueNPC).EnablePuzzlesNode == node.Key){
                ((StoryNPC)DialogueNPC).InvokeEnablePuzzles();
            }
        }
        else if(DialogueNPC is FillerNPC) {
            if(((FillerNPC)DialogueNPC).OpenDoorNode == node.Key) {
                ((FillerNPC)DialogueNPC).InvokeDialogueDoorOpen();
            }
        }
    }

    public static void ShowResponses(DialogueNode currentNode) {
        CleanupSpeechUI();
        GameObject button;
        //If the first node response key contains a #, then its a special event for the ui to handle.
        if (currentNode.ResponseNodes.Count > 0) {
            if (currentNode.ResponseNodes[0].Contains("#")) {
                if (currentNode.ResponseNodes[0].Contains("#")) {
                    button = (GameObject)Instantiate(dialogueUI.ResponseButton, dialogueUI.ResponseButtonArea.transform);
                    responseButtons.Add(button);
                    button.GetComponent<Button>().GetComponentInChildren<Text>().text = "[View Cutscene]";
                    button.GetComponent<UIEventFowarder>().NodeEvent += DialogueController.GetEventNodeDelegate(currentNode.ResponseNodes[0]);
                    button.SetActive(true);
                }
            }
            //response nodes are normal, get responses normally.
            else {
                List<DialogueNode> responses = DialogueController.GetNodeResponses(currentNode);
                foreach (DialogueNode response in responses) {
                    if (response.IsMemoryResponse) Debug.Log("Memory Response: " + response.ResponseText);
                    button = (GameObject)Instantiate(dialogueUI.ResponseButton, dialogueUI.ResponseButtonArea.transform);
                    responseButtons.Add(button);
                    button.GetComponent<Button>().GetComponentInChildren<Text>().text = response.ResponseText;
                    button.GetComponent<UIEventFowarder>().Buttonevent += HandleDialogueResponse;
                    button.name = response.Key;
                    button.SetActive(true);
                }
            }
        }
        else {
            //If there are no dialogue responses left, we allow the user to exit now.
            if (!allowExit) {
                dialogueUI.ExitButton.SetActive(true);
                allowExit = true;
            }
        }
        UnlockCursor();
        LockPlayerController();
    }

    public void ExitSpeechUI() {
        if (allowExit) {
            dialogueUI.Parent.SetActive(false);
            MenuOpen = false;
            dialogueUIOpen = false;
            LockCursor();
            UnlockPlayerController();

            //We award a memory here, this assumes that the dialogue was closed ONLY this way.

            if (DialogueMemoryCount >= DialogueMemoryTotal && DialogueMemoryTotal != -1) {
                Player.player.inventory.AddItem(DialogueMemoryID, 1);
                UI.ShowMessage("You were awarded the memory: " + Item.GetItem(DialogueMemoryID).Name);
            } 
        }
    }

    public void ProgressDialogue() {
        //To advance the dialogue if there is too much to display or its split.
    }

    public static void HandleDialogueResponse(GameObject originalObject) {
        DialogueNode node = DialogueController.GetNode(originalObject.name);
        if (node.IsMemoryResponse) DialogueMemoryCount++;
        ContinueDialogueConversation(node);
    }

    public static void CleanupSpeechUI() {
        if (responseButtons.Count > 0) {
            for (int i = responseButtons.Count; i > 0; i--) {
                Destroy(responseButtons[i-1]);
            } 
        }
    }
    #endregion

    //Inventory Code
    #region
    public static void UpdateInventoryUI() {
        GameObject newInvItem;
        UIItemSlot newSlot;
        inventoryUI.ItemInformationPanel.SetActive(false);

        List<Inventory.ItemSlot> popItemSlots = Player.player.inventory.GetPopulatedItemSlots();
        if (popItemSlots.Count < 0) return;
        foreach (Inventory.ItemSlot itemSlot in popItemSlots) {
            newInvItem = (GameObject)Instantiate(inventoryUI.BaseInventoryItem, inventoryUI.InventoryItemArea.transform);
            inventoryItems.Add(newInvItem);
            newSlot = newInvItem.GetComponent<UIItemSlot>();
            Item item = Item.GetItem(itemSlot.ItemID);
            newSlot.item = item;
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
        inventoryUI.ItemInfoViewMemoryButton.SetActive(false);
        selectedItem = slot.item;
        inventoryUI.ItemInfoName.text = slot.ItemName.text;
        inventoryUI.ItemInfoType.text = slot.ItemType.text;
        inventoryUI.ItemInfoQuantity.text = slot.ItemQuantity.text;
        inventoryUI.ItemInfoSprite.sprite = slot.ItemSprite.sprite;
        inventoryUI.ItemInfoDescription.text = slot.item.Description;
        if(slot.item is Memory) inventoryUI.ItemInfoViewMemoryButton.SetActive(true);
        if (!slot.item.Dropable) inventoryUI.ItemInfoDropItemButton.GetComponent<Button>().interactable = false;
        else inventoryUI.ItemInfoDropItemButton.GetComponent<Button>().interactable = true;
        inventoryUI.ItemInformationPanel.SetActive(true);
    }

    public void ItemDrop() {
        Player.player.inventory.RemoveItem(selectedItem.ID, 1);
        CleanInventoryUI();
        UpdateInventoryUI();
    }
    
    public void ViewMemory() {
        inventoryUI.Memory.sprite = ((Memory)selectedItem).MemorySprite;
        inventoryUI.MemoryName.text = selectedItem.Name;
        inventoryUI.MemoryParent.SetActive(true);
        inventoryUI.MemoryCodeDigit.text = ((Memory)selectedItem).CodeDigit.ToString();
    }

    public void CloseMemory() {
        inventoryUI.MemoryParent.SetActive(false);
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
            MenuOpen = true;
            allowExit = true;
            puzzle2DOpen = true;
        }
    }

    public static void Hide2DPuzzle() {
        if (puzzle2DOpen) {
            LockCursor();
            UnlockPlayerController();
            puzzle2D.SetActive(false);
            MenuOpen = false;
            puzzle2DOpen = false;
        }
    }
    #endregion

    //Cutscene code
    #region
    public static void StartImageCutscene(List<Sprite> cutsceneSprites, string postCutSceneDialogueNodeKey) {
        ui.ExitSpeechUI();
        MenuOpen = true;
        CutsceneSprites = cutsceneSprites;
        CutsceneImage.sprite = CutsceneSprites[0];
        PostCutSceneDialogueNodeKey = postCutSceneDialogueNodeKey;
        CutsceneObject.SetActive(true);
        if(CutsceneSprites.Count > 1) {
            CutsceneContButton.SetActive(true);
            CutsceneExitButton.SetActive(false);
        }
        else {
            CutsceneContButton.SetActive(false);
            CutsceneExitButton.SetActive(true);
        }
        LockPlayerController();
        UnlockCursor();
    }

    public void ContinueCutscene() {
        CutsceneSprites.RemoveAt(0);
        CutsceneImage.sprite = CutsceneSprites[0];
        if (CutsceneSprites.Count > 1) {
            CutsceneContButton.SetActive(true);
            CutsceneExitButton.SetActive(false);
        }
        else {
            CutsceneContButton.SetActive(false);
            CutsceneExitButton.SetActive(true);
        }
    }

    public void ExitCutscene() {
        MenuOpen = false;
        CutsceneObject.SetActive(false);
        CutsceneSprites = new List<Sprite>();
        if(PostCutSceneDialogueNodeKey != null && PostCutSceneDialogueNodeKey != "") {
            NewDialogueConversation(DialogueController.GetNode(PostCutSceneDialogueNodeKey), false);
        }
        else {
            UnlockPlayerController();
            LockCursor();
        }
    }
    #endregion

    //Inner dialogue
    #region
    public static void InnerDialogue(List<string> innerDialogueText, string name) {
        MenuOpen = true;
        allowExit = false;
        innerDialogueTextSequence = innerDialogueText;
        InnerDialogueName.text = name + ":";
        InnerDialogueText.text = "\"" + innerDialogueTextSequence[0] + "\"";
        innerDialogueTextSequence.RemoveAt(0);
        if (innerDialogueTextSequence.Count > 0) {
            InnerDialogueContButton.SetActive(true);
            InnerDialogueExitButton.SetActive(false);
        }
        else {
            InnerDialogueContButton.SetActive(false);
            InnerDialogueExitButton.SetActive(true);
        }
        InnerDialogueObject.SetActive(true);
        LockPlayerController();
        UnlockCursor();
    }

    public void ContinueInnerDialogueText() {
        InnerDialogueText.text = "\"" + innerDialogueTextSequence[0] + "\"";
        innerDialogueTextSequence.RemoveAt(0);
        if (innerDialogueTextSequence.Count > 0) {
            InnerDialogueContButton.SetActive(true);
            InnerDialogueExitButton.SetActive(false);
        }
        else {
            InnerDialogueContButton.SetActive(false);
            InnerDialogueExitButton.SetActive(true);
        }
    }

    public void CloseInnerDialogue() {
        InnerDialogueObject.SetActive(false);
        MenuOpen = false;
        UnlockPlayerController();
        LockCursor();
    }
    #endregion

    public static void Ending() {
        MenuOpen = true;
        allowExit = false;
        UnlockCursor();
        LockPlayerController();
        Progression.EndingType end = Progression.GetEndingType();
        if(end == Progression.EndingType.True) {
            EndingImage.sprite = Resources.Load<Sprite>("EndImage/heaven");
            EndingText.text = "\"Welcome to Heaven\"";
            EndingImage.gameObject.SetActive(true);
        }
        else if(end == Progression.EndingType.Neutral) {
            EndingImage.color = Color.black;
            EndingText.text = "\"Looks like you will need to start over to do better...\"";
            EndingImage.gameObject.SetActive(true);
        }
        else {
            //Bad ending
            EndingImage.sprite = Resources.Load<Sprite>("EndImage/hell");
            EndingText.text = "\"Welcome to Hell\"";
            EndingImage.gameObject.SetActive(true);
        }
    }

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
}

[System.Serializable]
public struct DialogueUI {
    public GameObject Parent;
    public InputField MainTextArea;
    public GameObject ResponseButtonArea;
    public GameObject ResponseButton;
    public GameObject ExitButton;
}

[System.Serializable]
public struct InventoryUI
{
    public GameObject Parent;
    public GameObject BaseInventoryItem;
    public GameObject InventoryItemArea;

    public GameObject ItemInformationPanel;
    public Text ItemInfoName;
    public Text ItemInfoType;
    public Text ItemInfoQuantity;
    public Image ItemInfoSprite;
    public Text ItemInfoDescription;
    public GameObject ItemInfoViewMemoryButton;
    public GameObject ItemInfoDropItemButton;

    public GameObject MemoryParent;
    public Image Memory;
    public Text MemoryName;
    public Text MemoryCodeDigit;
}
