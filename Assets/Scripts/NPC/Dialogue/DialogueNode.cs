using System.Collections.Generic;

public class DialogueNode {
    public string Text = "";
    public string Name = "New Node";
    public string Optiontitle;
    //key is the shown text on the response button, value is the name of the corrisponding dialoguenode.
    public Dictionary<string, string> Responses = new Dictionary<string, string>();

    public DialogueNode(string name, string text) {
        Name = name;
        Text = text;
    }
}
