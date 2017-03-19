using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq.Expressions;


public class DeleteUI : EditorWindow
{
	public Dictionary<FileInfo, IUnityXMLSerialisable> InstancePairs { get; set; }
	private static int _selected = 0;
	private static DeleteUI _deleteWindow;

	void OnGUI()
	{
		if (InstancePairs != null && InstancePairs.Count != 0)
		{
			string[] options = InstancePairs.Select(x => x.Value.GetDisplayValue()).ToArray();
			_selected = EditorGUILayout.Popup("Label", _selected, options);
			if (GUILayout.Button("Delete"))
			{
				KeyValuePair<FileInfo, IUnityXMLSerialisable> result = InstancePairs.Select(x => x).Where(x => x.Value.GetDisplayValue() == options[_selected]).First();
				InstancePairs.Remove(result.Key);
				File.Delete(result.Key.FullName);
				_selected = 0;
			}
		}
	}

}

