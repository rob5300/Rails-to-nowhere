using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

public class SerialiserUI : EditorWindow
{
	string myString = "New NPC";
	bool groupEnabled;
	bool myBool = true;
	float myFloat = 1.23f;
	private static Dictionary<IUnityXMLSerialisable, List<UnityEngine.Object>> _serialiseableObjectFields;
	private static SerialiserUI _window;

	// Add menu named "My Window" to the Window menu
	[MenuItem("Window/Unity-To-XML Serialiser")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		_window = GetWindow<SerialiserUI>();
		_window.titleContent.text = "U2XML";
		_window.Show();
	}

	void OnGUI()
	{
		_serialiseableObjectFields = new Dictionary<IUnityXMLSerialisable, List<UnityEngine.Object>>();
		DynamicallyLoadSerialisedFiles();

		//myString = EditorGUILayout.TextField("Text Field", myString);
		//EditorGUILayout.
		//groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
		//myBool = EditorGUILayout.Toggle("Toggle", myBool);
		//myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
		//EditorGUILayout.EndToggleGroup();
	}

	private static void DynamicallyLoadSerialisedFiles()
	{
		Type baseInterfaceType = typeof(IUnityXMLSerialisable);
		Type baseSerialiserType = typeof(UnityXMLSerialiser<>);
		List<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(y => baseInterfaceType.IsAssignableFrom(y) && y != baseInterfaceType && !y.IsAbstract).ToList();
		foreach (Type serialiseableType in types)
		{
			GUILayout.Label(serialiseableType.Name + " Instances", EditorStyles.boldLabel);
			Type actualSerialiserType = baseSerialiserType.MakeGenericType(serialiseableType);
			object instantiatedSerialiser = Activator.CreateInstance(actualSerialiserType);
			List<string> appropriateFiles = Directory.GetFiles(Application.streamingAssetsPath).Where(x => x.Contains(serialiseableType.FullName) && !x.Contains(".meta")).ToList();

			foreach (string file in appropriateFiles)
			{
				string actualPath = file.Replace('/', '\\');
				FileInfo fileInfo = new FileInfo(actualPath);
				IUnityXMLSerialisable result = (IUnityXMLSerialisable)instantiatedSerialiser.GetType().GetMethod("DeserialiseXML").Invoke(instantiatedSerialiser, new object[2] { fileInfo, false });
				List<string> targetProps = result.GetSerialiseTargets();
				if (!_serialiseableObjectFields.Keys.Contains(result))
				{
					_serialiseableObjectFields.Add(result, new List<UnityEngine.Object>());
				}
				foreach (string prop in targetProps)
				{
					List<UnityEngine.Object> existingFields = _serialiseableObjectFields[result];
					if (!existingFields.Where(x => x.name == prop).Any())
					{
						//var bla = EditorGUILayout.ObjectField(result.GetType().GetProperty(prop).GetValue(result, null), objType: result.GetType().GetProperty(prop).GetType(), allowSceneObjects: true);
						//var bla = EditorGUILayout.ObjectField(label: "bla", obj: result.GetSerialiseTargets(), objType: typeof(List<String>), allowSceneObjects: true);
						//	existingFields.Add(bla);
					}
				}
			}

		}
	}
}
