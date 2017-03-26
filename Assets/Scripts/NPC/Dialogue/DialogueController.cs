using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class DialogueController {

    private delegate string PlaceHolderNode();

    private static Dictionary<string, DialogueNode> _nodeLibrary = new Dictionary<string, DialogueNode>();
    private static Dictionary<string, PlaceHolderNode> _placeholderNodes = new Dictionary<string, PlaceHolderNode>();

    public static Dictionary<string, DialogueNode> NodeDictionary {
        get { return _nodeLibrary; }
    }

    ////For testing purposes.
    static DialogueController() {
        ////Dialogue nodes can be added with just the constructor and AddDialogueNode. Make sure to check the constructors to see which suits your new node best.
        _placeholderNodes.Add("$endingbranch", delegate(){ return Progression.GetEndingBranchDialogueNode(); });
        LoadDictionary();
    }

    public static void AddDialogueNode(DialogueNode node) {
        if (NodeDictionary.ContainsKey(node.Key)) {
            return;
        }
        NodeDictionary.Add(node.Key, node);
    }

    public static DialogueNode GetNode(string nodeKey) {
        //If this node is a placeholder node, get the actual node from it.
        if (nodeKey.Contains("$")) {
            return GetPlaceholderNode(nodeKey);
        }

        if (!NodeDictionary.ContainsKey(nodeKey)) {
            Debug.LogError("Dialogue Node '" + nodeKey + "' was not found.");
            return null;
        }
        return NodeDictionary[nodeKey.ToLower()];
    }

    private static DialogueNode GetPlaceholderNode(string placeholderNode) {
        if (!_placeholderNodes.ContainsKey(placeholderNode)) {
            Debug.Log("Placeholder node was not found. (" + placeholderNode + ")");
            return null;
        }
        return GetNode(_placeholderNodes[placeholderNode].Invoke());
    }

    public static void DeleteNode(DialogueNode node) {
        NodeDictionary.Remove(NodeDictionary.Where(pair => pair.Value == node).First().Key);
    }

    public static void DeleteNode(string key) {
        NodeDictionary.Remove(key);
    }

    public static void SaveDictionary() {
        FileInfo info = new FileInfo(Application.streamingAssetsPath.Replace('/', '\\') + "\\" + "DialogueNodes.xml");
        NodeDictionary.ToList().SerialiseSingularNonGameObject(info.FullName);
    }

    public static void LoadDictionary() {
        FileInfo info = new FileInfo(Application.streamingAssetsPath.Replace('/', '\\') + "\\" + "DialogueNodes.xml");
        List<KeyValuePair<string, DialogueNode>> nodeList = new List<KeyValuePair<string, DialogueNode>>().DeserialiseSingularNonGameObject(info.FullName);
        _nodeLibrary = new Dictionary<string, DialogueNode>();
        foreach (KeyValuePair<string, DialogueNode> pair in nodeList) {
            NodeDictionary.Add(pair.Key, pair.Value);
        }
    }

    public static List<DialogueNode> GetNodeResponses(DialogueNode node) {
        List<DialogueNode> responses = new List<DialogueNode>();
        foreach (string key in node.ResponseNodes) {
            DialogueNode response = GetNode(key);
            if(response != null) {
                responses.Add(GetNode(key));
            }
        }

        return responses;
    }

    public static List<DialogueNode> GetNodeResponses(string nodeKey) {
        DialogueNode node = GetNode(nodeKey);
        return GetNodeResponses(node);
    }
}
