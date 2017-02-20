using System.Collections.Generic;

public static class DialogueController{

    private static Dictionary<string, DialogueNode> _NodeLibrary = new Dictionary<string, DialogueNode>();

    //For testing purposes.
    static DialogueController() {
        DialogueNode test01 = new DialogueNode("_", "This is a test text node.");
        test01.AddResponse("test02");
        test01.AddResponse("test03");
        AddDialogueNode("test01", test01);
        AddDialogueNode("test02", new DialogueNode("response 1", "This is a response node, 1."));
        AddDialogueNode("test03", new DialogueNode("response 2", "This is a response node, 2."));
    }

    public static void AddDialogueNode(string nodeName, DialogueNode node) {
        if (_NodeLibrary.ContainsKey(nodeName)) {
            return;
        }
        _NodeLibrary.Add(nodeName, node);
    }

    public static DialogueNode GetNode(string nodeName) {
        return _NodeLibrary[nodeName];
    }

    public static List<DialogueNode> GetNodeResponses(DialogueNode node) {
        List<DialogueNode> responses = new List<DialogueNode>();
        foreach(string key in node.Responses) {
            responses.Add(GetNode(key));
        }

        return responses;
    }

    public static List<DialogueNode> GetNodeResponses(string nodename) {
        DialogueNode node = GetNode(nodename);
        return GetNodeResponses(node);
    }
}
