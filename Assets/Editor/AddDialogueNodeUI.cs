using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class AddDialogueNodeUI : EditorWindow
{
	private static AddDialogueNodeUI _window;
	private static string _key;
	private static string _text;
	private static bool _isMemory;
	private static List<string> _responseNodes;
	private static int _amountOfResponseNodes;
	private static string _responseText;

	[MenuItem("Window/Add new DialogueNode...")]
	static void Init () {
		_window = GetWindow<AddDialogueNodeUI>();
		_window.titleContent.text = "Add Node";
		_window.Show();
	}
	
	// Update is called once per frame
	void OnGUI () {
		bool add = false;
		if (GUILayout.Button("Export Nodes"))
		{
			DialogueController.SaveDictionary();
		}
		if (GUILayout.Button("Add Node"))
		{
			add = true;
		}
		_key = EditorGUILayout.TextField("Node Key:", _key);
		_text = EditorGUILayout.TextField("Node Text:", _text);
		_responseText = EditorGUILayout.TextField("Node Response Text:", _responseText);
		_isMemory = EditorGUILayout.Toggle("Is Memory Node:", _isMemory);
		_amountOfResponseNodes = EditorGUILayout.IntField("Amount of Response Nodes:", _amountOfResponseNodes);
		if (_responseNodes != null)
		{
			if (_responseNodes.Count > 0)
			{
				List<string> oldNodes = new List<string>();
				oldNodes = _responseNodes;
				_responseNodes = new List<string>();
				for (int i = 0; i < _amountOfResponseNodes; i++)
				{
					_responseNodes.Add(oldNodes[i]);
				}
			}
			else
			{
				for (int i = 0; i < _amountOfResponseNodes; i++)
				{
					_responseNodes.Add("");
				}
			}

		}
		else
		{
			_responseNodes = new List<string>();
			for (int i = 0; i < _amountOfResponseNodes; i++)
			{
				_responseNodes.Add("");
			}
		}

		for (int i = 0; i < _responseNodes.Count; i++)
		{
			_responseNodes[i] = EditorGUILayout.TextField("Node Response " + i, _responseNodes[i]);
		}
		if (add)
		{
			DialogueNode node = new DialogueNode(_key, _text, _responseText, _responseNodes, _isMemory);
			DialogueController.AddDialogueNode(node);
			_amountOfResponseNodes = 0;
			_isMemory = false;
			_key = "";
			_responseText = "";
			_text = "";
		}
	}
}
