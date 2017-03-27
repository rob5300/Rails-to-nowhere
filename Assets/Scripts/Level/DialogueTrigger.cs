using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class DialogueTrigger : MonoBehaviour {

    public bool hasbeenactivated = false;
    public bool CanExitDialogue = true;
    public string dialogueNode = "";

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player" && !hasbeenactivated) {
            hasbeenactivated = true;
            UI.NewDialogueConversation(DialogueController.GetNode(dialogueNode), CanExitDialogue);
        }
    }

}
