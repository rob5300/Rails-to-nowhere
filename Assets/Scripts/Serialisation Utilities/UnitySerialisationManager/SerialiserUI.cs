using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq.Expressions;

public class SerialiserUI : EditorWindow
{
	private static int _oldCount = 0;
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
			Type baseInterfaceType = typeof(IUnityXMLSerialisable);
			Type baseSerialiserType = typeof(UnityXMLSerialiser<>);
			List<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(y => baseInterfaceType.IsAssignableFrom(y) && y != baseInterfaceType && !y.IsAbstract).ToList();
			foreach (Type serialiseableType in types)
			{
				GUILayout.Label(serialiseableType.Name + " Instances", EditorStyles.boldLabel);
				foreach (IUnityXMLSerialisable instance in _instances.Where(x => x.GetType() == serialiseableType))
				{
					DrawFields(instance);
				}
			}
		}
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
			PropertyInfo propInfo = target.GetType().GetProperty(prop);
			List<Expression<Func<object, object>>> anonymousMethods = target.GetMappings(prop);
			if (propInfo.PropertyType.IsAssignableFrom(typeof(UnityEngine.Object)))
			{
				propInfo.SetValue(target, EditorGUILayout.ObjectField(prop + ":", (UnityEngine.Object)propInfo.GetValue(target, null), target.GetType(), true), null);
			}
			else if (propInfo.PropertyType.IsPrimitive || propInfo.PropertyType == typeof(string))
			{
				MethodInfo method = GetFieldMethod(target, prop, propInfo.PropertyType);
				propInfo.SetValue(target, method.Invoke(null, new object[] { prop + ":", propInfo.GetValue(target, null), new GUILayoutOption[0] }), null);
			}
			else if (anonymousMethods != null && !propInfo.PropertyType.IsAssignableFrom(typeof(IList)))
			{
				RunAnonymousMethodDrawing(target, propInfo, anonymousMethods, propInfo.GetValue(target, null));
			}
			else
			{
				if (propInfo.PropertyType.IsAssignableFrom(typeof(IList)))
				{
					IList resultList = (IList)propInfo.GetValue(target, null);
					if (propInfo.PropertyType.GetElementType().IsAssignableFrom(typeof(UnityEngine.Object)))
					{
						for (int i = 0; i < resultList.Count; i++)
						{
							resultList[i] = EditorGUILayout.ObjectField(prop + " - " + i + ":", (UnityEngine.Object)resultList[i], target.GetType(), true);
						}
					}
					else if (propInfo.PropertyType.GetElementType().IsPrimitive || propInfo.PropertyType.GetElementType() == typeof(string))
					{
						for (int i = 0; i < resultList.Count; i++)
						{
							MethodInfo method = GetFieldMethod(target, prop, propInfo.PropertyType);
							resultList[i] = method.Invoke(null, new object[] { prop + " - " + i + ":", propInfo.GetValue(target, null), new GUILayoutOption[0] });
						}
					}
					else if (anonymousMethods != null)
					{
						RunAnonymousMethodDrawing(target, propInfo, anonymousMethods, resultList);
					}
					else
					{
						Debug.Log("I died in a fire!");
					}
				}

			}
		}
		EditorGUILayout.Space();
	}

	private static void RunAnonymousMethodDrawing(IUnityXMLSerialisable target, PropertyInfo prop, List<Func<object, object>> anonymousMethods, IList collection)
	{

		for (int i = 0; i < collection.Count; i++)
		{
			foreach (Func<object, object> methodToRun in anonymousMethods)
			{
				object result = methodToRun.Invoke(collection[i]);
				if (result.GetType().IsPrimitive || result.GetType() == typeof(string))
				{
					MethodInfo method = GetFieldMethod(target, prop.Name, result.GetType());
					result = method.Invoke(null, new object[] { prop + " - " + result.GetType().Name + ", index " + i + ":", result, new GUILayoutOption[0] });
				}
				else if (result.GetType().IsAssignableFrom(typeof(IList)))
				{
					RunAnonymousMethodDrawing(target, prop, target.GetMappings(result.GetType().Name), (IList)result);
				}
				else
				{
					RunAnonymousMethodDrawing(target, prop, target.GetMappings(result.GetType().Name), result);
				}

			}
		}
	}

	private static void RunAnonymousMethodDrawing(IUnityXMLSerialisable target, PropertyInfo prop, List<Expression<Func<object, object>>> anonymousMethods, object targetObject)
	{
		foreach (Expression<Func<object, object>> methodToRun in anonymousMethods)
		{
			MemberExpression expr = (MemberExpression)methodToRun.Body;
			string memberName = expr.Member.Name;
			if (prop.PropertyType.GetProperties().Where(x => x.Name == memberName).Any())
			{
				PropertyInfo subProp = prop.PropertyType.GetProperty(memberName);
				object result = methodToRun.Compile().Invoke(targetObject);
				if (result != null)
				{
					if (result.GetType().IsPrimitive || result.GetType() == typeof(string))
					{
						MethodInfo method = GetFieldMethod(target, prop.Name, result.GetType());
						object parent = prop.GetValue(target, null);
						subProp.SetValue(parent, method.Invoke(null, new object[] { prop.Name + "." + memberName + ":", subProp.GetValue(parent, null), new GUILayoutOption[0] }), null);
					}
					else if (result.GetType().IsAssignableFrom(typeof(IList)))
					{
						RunAnonymousMethodDrawing(target, prop, target.GetMappings(result.GetType().Name), (IList)result);
					}
					else
					{
						RunAnonymousMethodDrawing(target, prop, target.GetMappings(result.GetType().Name), result);
					}
				}
			}
			else
			{
				Debug.Log("Mapping error: Cannot locate Primitive type for " + memberName + " - did you write the map correctly?");
			}

		}
	}

	private static MethodInfo GetFieldMethod(IUnityXMLSerialisable target, string prop, Type propInfo)
	{
		var methodGroup = typeof(EditorGUILayout).GetMethods().Where(x => (x.Name.Contains("Field") || x.Name.Contains("Toggle")) && x.IsStatic && !x.Name.Contains("Delay") && (x.ReturnType.IsPrimitive || x.ReturnType == typeof(string)) && !x.Name.Contains("Property"));
		if (methodGroup.Where(x => x.ReturnType == propInfo).Any())
		{
			MethodInfo methodToInvoke = methodGroup
								.Where(x => x.ReturnType == propInfo && x.GetParameters().Count() == 3 && x.GetParameters()
										.Where(y => y.GetCustomAttributes(typeof(ParamArrayAttribute), false).Length > 0).Any() && x.GetParameters()
										.Where(z => z.Name == "label").Any()).First();
			return methodToInvoke;
		}
		else
		{
			return null;
		}
	}
}

