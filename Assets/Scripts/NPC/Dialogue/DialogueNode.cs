using System.Collections.Generic;

public class DialogueNode {
    public string Text = "";
    public string Key = "New Node";
    public string ResponseText;
    //key is the shown text on the response button, value is the name of the corrisponding dialoguenode.
    public List<string> ResponseNodes = new List<string>();

    public DialogueNode(string key, string text) {
        Key = key.ToLower();
        Text = text;
    }

    public DialogueNode(string key, string text, string responsetext) {
        Key = key.ToLower();
        Text = text;
        ResponseText = responsetext;
    }

    public void AddResponse(string key) {
        if (ResponseNodes.Contains(key)) return;
        ResponseNodes.Add(key);
    }
}
