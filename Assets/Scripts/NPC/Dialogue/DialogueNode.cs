using System.Collections.Generic;

public class DialogueNode {
    private string _text = "";
    private string _key = "New Node";
    private bool isMemoryResponse = false;
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

    public bool IsMemoryResponse {
        get {
            return isMemoryResponse;
        }

        set {
            isMemoryResponse = value;
        }
    }

    public DialogueNode()
	{

	}

    public DialogueNode(string key, string text) {
        Key = key.ToLower();
        Text = text;
    }

    public DialogueNode(string key, string text, string responsetext) : this(key, text){
        ResponseText = responsetext;
    }

    public DialogueNode(string key, string text, string responsetext, List<string> responseNodes) : this(key, text, responsetext) {
        List<string> lowerN = new List<string>();
        foreach(string n in responseNodes) {
            lowerN.Add(n.ToLower());
        }
        ResponseNodes = lowerN;
    }

    public DialogueNode(string key, string text, string responsetext, List<string> responseNodes, bool memoryResponse) : this(key, text, responsetext, responseNodes) {
        IsMemoryResponse = memoryResponse;
    }

    public DialogueNode(string key, string text, string responsetext, bool memoryResponse) : this(key, text, responsetext) {
        IsMemoryResponse = memoryResponse;
    }

    public void AddResponse(string key) {
        if (ResponseNodes.Contains(key)) return;
        ResponseNodes.Add(key.ToLower());
    }
}
