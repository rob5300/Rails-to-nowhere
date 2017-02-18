using System.Collections.Generic;

public class DialogueNode {
    public string Text = "";
    public string Name = "New Node";
    public string Optiontitle;
    //key is the shown text on the response button, value is the name of the corrisponding dialoguenode.
    public List<string> Responses = new List<string>();

    public DialogueNode(string name, string text) {
        Name = name;
        Text = text;
    }

    public void AddResponse(string key) {
        if (Responses.Contains(key)) return;
        Responses.Add(key);
    }
}
