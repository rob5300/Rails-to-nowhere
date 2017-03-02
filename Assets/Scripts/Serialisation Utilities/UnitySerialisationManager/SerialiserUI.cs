using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

public class SerialiserUI : EditorWindow
{
	private static int _oldCount = 0;
	string myString = "New NPC";
	bool groupEnabled;
	bool myBool = true;
	float myFloat = 1.23f;
	private static Dictionary<IUnityXMLSerialisable, List<UnityEngine.Object>> _serialiseableUnityObjectFields;
	private static Dictionary<IUnityXMLSerialisable, List<object>> _serialisableObjectFields;
	private static List<IUnityXMLSerialisable> _instances;
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
		int newCount = Directory.GetFiles(Application.streamingAssetsPath).Where(x => x.Contains(".xml")).Count();

		if (_instances == null || newCount != _oldCount)
		{
			IUnityXMLSerialisable[] resultColl = GameObject.Find("InstanceHolderObj").GetComponents<IUnityXMLSerialisable>();
			foreach (var behaviour in resultColl)
			{
				DestroyImmediate((MonoBehaviour)behaviour);
			}
			DynamicallyLoadSerialisedFiles();
			_oldCount = newCount;
		}
		else
		{
			foreach (IUnityXMLSerialisable instance in _instances)
			{
				DrawFields(instance);
			}
		}



		//myString = EditorGUILayout.TextField("Text Field", myString);
		//ditorGUILayout.Toggle()
		//EditorGUILayout.
		//groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
		//myBool = EditorGUILayout.Toggle("Toggle", myBool);
		//myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
		//EditorGUILayout.EndToggleGroup();
	}

	private static void DynamicallyLoadSerialisedFiles()
	{
		_instances = new List<IUnityXMLSerialisable>();
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
				DrawFields(result);
			}

		}
	}

	private static void DrawFields(IUnityXMLSerialisable target)
	{
		List<string> targetProps = target.GetSerialiseTargets();
		if (!_instances.Contains(target))
		{
			_instances.Add(target);
		}
		foreach (string prop in targetProps)
		{
			//if (!existingUnityFields.Where(x => x.name == prop).Any())
			//{
			PropertyInfo propInfo = target.GetType().GetProperty(prop);
			if (propInfo.PropertyType.IsAssignableFrom(typeof(UnityEngine.Object)))
			{
				propInfo.SetValue(target, EditorGUILayout.ObjectField(prop + ":", (UnityEngine.Object)propInfo.GetValue(target, null), target.GetType(), true), null);
			}
			else if (propInfo.PropertyType.IsPrimitive || propInfo.PropertyType == typeof(string))
			{
				var methodGroup = typeof(EditorGUILayout).GetMethods().Where(x => (x.Name.Contains("Field") || x.Name.Contains("Toggle")) && x.IsStatic && !x.Name.Contains("Delay") && (x.ReturnType.IsPrimitive || x.ReturnType == typeof(string)) && !x.Name.Contains("Property"));
				if (methodGroup.Where(x => x.ReturnType == propInfo.PropertyType).Any())
				{
					var methodToInvoke = methodGroup
										.Where(x => x.ReturnType == propInfo.PropertyType && x.GetParameters().Count() == 3 && x.GetParameters()
												.Where(y => y.GetCustomAttributes(typeof(ParamArrayAttribute), false).Length > 0).Any() && x.GetParameters()
												.Where(z => z.Name == "label").Any()).First();
					propInfo.SetValue(target, methodToInvoke.Invoke(null, new object[] { prop + ":", propInfo.GetValue(target, null), new GUILayoutOption[0] }), null);
				}
				else
				{
					Debug.Log("Non-monobehaviour type could not be serialised. Reason: Not primitive.");
				}
			}
			//this.Repaint();
		}
		EditorGUILayout.Space();
	}

	//void OnValidate()
	//{
	//	foreach (IUnityXMLSerialisable instance in _instances)
	//	{
	//		DrawFields(instance);
	//	}
	//}
}
