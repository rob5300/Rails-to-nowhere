using System.Collections.Generic;

public static class DialogueController{

    private static Dictionary<string, DialogueNode> _nodeLibrary = new Dictionary<string, DialogueNode>();

	public static Dictionary<string, DialogueNode> NodeDictionary
	{
		get { return _nodeLibrary; }
	}   

	//For testing purposes.
	static DialogueController() {
        DialogueNode test01 = new DialogueNode("test01", "This is a test text node.", "Back to test01");
        test01.AddResponse("response1");
        test01.AddResponse("response2");
        test01.AddResponse("response3");
        AddDialogueNode(test01);
        AddDialogueNode(new DialogueNode("response1", "This is a response node, 1.", "Response 01"));
        AddDialogueNode(new DialogueNode("response2", "This is a response node, 2.", "Response 02"));
        AddDialogueNode(new DialogueNode("response3", "This is a response node with no extra nodes to respond with, soo no buttons appear", "Response with no responses"));
        GetNode("response1").AddResponse("test01");
        GetNode("response2").AddResponse("test01");
    }

    public static void AddDialogueNode(DialogueNode node) {
        if (_nodeLibrary.ContainsKey(node.Key)) {
            return;
        }
        _nodeLibrary.Add(node.Key, node);
    }

    public static DialogueNode GetNode(string nodeKey) {
        return _nodeLibrary[nodeKey.ToLower()];
    }

    public static List<DialogueNode> GetNodeResponses(DialogueNode node) {
        List<DialogueNode> responses = new List<DialogueNode>();
        foreach(string key in node.ResponseNodes) {
            responses.Add(GetNode(key));
        }

        return responses;
    }

    public static List<DialogueNode> GetNodeResponses(string nodeKey) {
        DialogueNode node = GetNode(nodeKey);
        return GetNodeResponses(node);
    }
}
