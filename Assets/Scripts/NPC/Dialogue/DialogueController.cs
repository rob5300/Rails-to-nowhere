using System.Collections.Generic;

public static class DialogueController{

    private static Dictionary<string, DialogueNode> _nodeLibrary = new Dictionary<string, DialogueNode>();

	public static Dictionary<string, DialogueNode> NodeDictionary
	{
		get { return _nodeLibrary; }
	}   

	//For testing purposes.
	static DialogueController() {
        //Dialogue nodes can be added with just the constructor and AddDialogueNode. Make sure to check the constructors to see which suits your new node best.
        AddDialogueNode(new DialogueNode("test01", "This is a test text node.", "Back to test01", new List<string>() { "response1", "response2", "response3", "memoryNode" }));
        AddDialogueNode(new DialogueNode("response1", "This is a response node, 1.", "Response 01", new List<string>() { "test01" }));
        AddDialogueNode(new DialogueNode("response2", "This is a response node, 2.", "Response 02", new List<string>() { "test01" }));
        AddDialogueNode(new DialogueNode("response3", "This is a response node with no extra nodes to respond with, soo no buttons appear", "Response with no responses"));
        AddDialogueNode(new DialogueNode("memoryNode", "Here, have a memory!", "Award memory", true));
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
