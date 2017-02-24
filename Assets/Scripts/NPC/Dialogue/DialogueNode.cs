using System.Collections.Generic;

public class DialogueNode {
    private string _text = "";
    private string _key = "New Node";
    private string _responseText;
    public List<string> ResponseNodes = new List<string>();

    public string Text {
        get {
            return _text;
        }

        set {
            _text = value;
        }
    }

    public string Key {
        get {
            return _key;
        }

        set {
            _key = value;
        }
    }

    public string ResponseText {
        get {
            return _responseText;
        }

        set {
            _responseText = value;
        }
    }

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
