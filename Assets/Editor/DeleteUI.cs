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

	void OnGUI()
	{
		string[] options = InstancePairs.Select(x => (string)x.GetType().GetProperty(x.Value.GetDeleteDisplayProp()).GetValue(x, null)).ToArray();
		_selected = EditorGUILayout.Popup("Label", _selected, options);
		if (GUILayout.Button("Delete"))
		{
			KeyValuePair<FileInfo, IUnityXMLSerialisable> keyPairVals = InstancePairs.Select(x => x).Where(x => (string)x.GetType().GetProperty(x.Value.GetDeleteDisplayProp()).GetValue(x, null) == options[_selected]).First();
			File.Delete(keyPairVals.Key.FullName);
		}
	}

}

