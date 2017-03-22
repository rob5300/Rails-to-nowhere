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
	private static GameObject _templateInstance;
	private static bool _save = false;
	private static DeleteUI _deleteWindow;
	private static Dictionary<FileInfo, IUnityXMLSerialisable> _instancePairs;
	private static Dictionary<PropertyInfo, Dictionary<IUnityXMLSerialisable, int>> _listObjs;

	// Add menu named "My Window" to the Window menu
	[MenuItem("Window/Unity-To-XML Serialiser")]
	static void Init()
	{

		// Get existing open window or if none, make a new one:
		_window = GetWindow<SerialiserUI>();
		_window.titleContent.text = "U2XML";
		_window.Show();
		_templateInstance = GameObject.Find("InstanceHolderObj");

	}

	void OnGUI()
	{
		_save = false;
		if (_templateInstance == null)
		{
			_templateInstance = GameObject.Find("InstanceHolderObj");
		}
		if (GUILayout.Button("Save all"))
		{
			_save = true;
		}
		int newCount = Directory.GetFiles(Application.streamingAssetsPath).Where(x => x.Contains(".xml")).Count();
		if (GUILayout.Button("Delete an instance"))
		{
			_deleteWindow = GetWindow<DeleteUI>();
			_deleteWindow.titleContent.text = "Delete";
			_deleteWindow.InstancePairs = _instancePairs;
		}

		if (_instances == null || newCount != _oldCount)
		{
			IUnityXMLSerialisable[] resultColl = _templateInstance.GetComponents<IUnityXMLSerialisable>();
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
				if (GUILayout.Button("Add new " + serialiseableType.Name))
				{
					_instances.Add((IUnityXMLSerialisable)_templateInstance.AddComponent(serialiseableType));
				}
				foreach (IUnityXMLSerialisable instance in _instances.Where(x => x.GetType() == serialiseableType))
				{
					DrawFields(instance);
				}
			}
		}
		if (_save)
		{
			Type baseSerialiserType = typeof(UnityXMLSerialiser<>);
			Type baseInterfaceType = typeof(IUnityXMLSerialisable);
			List<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(y => baseInterfaceType.IsAssignableFrom(y) && y != baseInterfaceType && !y.IsAbstract).ToList();
			foreach (Type serialiseableType in types)
			{
				int i = 1;
				foreach (IUnityXMLSerialisable instanceToSave in _instances.Where(x => x.GetType() == serialiseableType))
				{

					Type actualSerialiserType = baseSerialiserType.MakeGenericType(instanceToSave.GetType());
					object instantiatedSerialiser = Activator.CreateInstance(actualSerialiserType);
					FileInfo info = new FileInfo(Application.streamingAssetsPath.Replace('/', '\\') + "\\" + instanceToSave.GetType().FullName + i + ".xml");
					instantiatedSerialiser.GetType().GetMethod("SerialiseInstance").Invoke(instantiatedSerialiser, new object[2] { instanceToSave, info });
					i++;
					//DestroyImmediate((MonoBehaviour)instanceToSave);
				}
			}
			//DynamicallyLoadSerialisedFiles();
		}
	}

	private static void DynamicallyLoadSerialisedFiles()
	{
		_instancePairs = new Dictionary<FileInfo, IUnityXMLSerialisable>();
		_instances = new List<IUnityXMLSerialisable>();
		Type baseInterfaceType = typeof(IUnityXMLSerialisable);
		Type baseSerialiserType = typeof(UnityXMLSerialiser<>);
		List<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(y => baseInterfaceType.IsAssignableFrom(y) && y != baseInterfaceType && !y.IsAbstract).ToList();
		foreach (Type serialiseableType in types)
		{
			GUILayout.Label(serialiseableType.Name + " Instances", EditorStyles.boldLabel);
			if (GUILayout.Button("Add new " + serialiseableType.Name))
			{
				_instances.Add((IUnityXMLSerialisable)_templateInstance.AddComponent(serialiseableType));
			}
			Type actualSerialiserType = baseSerialiserType.MakeGenericType(serialiseableType);
			object instantiatedSerialiser = Activator.CreateInstance(actualSerialiserType);
			List<string> appropriateFiles = Directory.GetFiles(Application.streamingAssetsPath).Where(x => x.Contains(serialiseableType.FullName) && !x.Contains(".meta")).ToList();

			foreach (string file in appropriateFiles)
			{
				string actualPath = file.Replace('/', '\\');
				FileInfo fileInfo = new FileInfo(actualPath);
				IUnityXMLSerialisable result = (IUnityXMLSerialisable)instantiatedSerialiser.GetType().GetMethod("DeserialiseXML").Invoke(instantiatedSerialiser, new object[2] { fileInfo, false });
				_instancePairs.Add(fileInfo, result);
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
			if (propInfo.PropertyType.IsSubclassOf(typeof(UnityEngine.Object)))
			{
				propInfo.SetValue(target, EditorGUILayout.ObjectField(prop + ":", (UnityEngine.Object)propInfo.GetValue(target, null), propInfo.PropertyType, true), null);
			}
			else if (propInfo.PropertyType.IsPrimitive || propInfo.PropertyType == typeof(string))
			{
				MethodInfo method = GetFieldMethod(target, prop, propInfo.PropertyType);
				propInfo.SetValue(target, method.Invoke(null, new object[] { prop + ":", propInfo.GetValue(target, null), new GUILayoutOption[0] }), null);
			}
			else if (anonymousMethods != null && propInfo.PropertyType.GetInterface("IList") == null)
			{
				RunAnonymousMethodDrawing(target, propInfo, anonymousMethods, propInfo.GetValue(target, null));
			}
			else
			{
				if (propInfo.PropertyType.GetInterface("IList") != null)
				{
					if (_listObjs == null)
					{
						_listObjs = new Dictionary<PropertyInfo, Dictionary<IUnityXMLSerialisable, int>>();
					}
					if (!_listObjs.Where(x => x.Key == propInfo).Any())
					{
						_listObjs.Add(propInfo, new Dictionary<IUnityXMLSerialisable, int>());
						_listObjs[propInfo][target] = 0;
					}
					if (!_listObjs[propInfo].Keys.Contains(target))
					{
						_listObjs[propInfo].Add(target, 0);
					}
					IList resultList = (IList)propInfo.GetValue(target, null);
					_listObjs[propInfo][target] = EditorGUILayout.IntField(propInfo.Name + " Capacity:", _listObjs[propInfo][target]);
					if (propInfo.PropertyType.IsArray)
					{
						if (propInfo.PropertyType.GetElementType().IsSubclassOf(typeof(UnityEngine.Object)))
						{
							if (resultList.Count < _listObjs[propInfo][target])
							{
								for (int i = resultList.Count; i < _listObjs[propInfo][target]; i++)
								{
									if (propInfo.PropertyType.GetGenericArguments()[0].IsSubclassOf(typeof(Component)))
									{
										resultList.Add(_templateInstance.AddComponent(resultList.GetType().GetElementType()));
									}
									else
									{
										GameObject oGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
										oGo.transform.position = _templateInstance.transform.position;
										oGo.transform.parent = _templateInstance.transform;
										resultList.Add(oGo);
									}
								}
							}
							for (int i = 0; i < _listObjs[propInfo][target]; i++)
							{
								UnityEngine.Object oldRef = (UnityEngine.Object)resultList[i];
								resultList[i] = EditorGUILayout.ObjectField(prop + " - " + i + ":", (UnityEngine.Object)resultList[i], propInfo.PropertyType.GetElementType(), true);
								if ((UnityEngine.Object)resultList[i] != oldRef)
								{
									DestroyImmediate(oldRef);
								}
							}
						}
						else if (propInfo.PropertyType.GetElementType().IsPrimitive || propInfo.PropertyType.GetElementType() == typeof(string))
						{
							if (resultList.Count < _listObjs[propInfo][target])
							{
								for (int i = resultList.Count; i < _listObjs[propInfo][target]; i++)
								{
									resultList.Add(Activator.CreateInstance(propInfo.PropertyType.GetElementType()));
								}
							}
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
					else
					{
						if (propInfo.PropertyType.GetGenericArguments()[0].IsSubclassOf(typeof(UnityEngine.Object)))
						{
							if (resultList.Count < _listObjs[propInfo][target])
							{
								for (int i = resultList.Count; i < _listObjs[propInfo][target]; i++)
								{
									if (propInfo.PropertyType.GetGenericArguments()[0].IsSubclassOf(typeof(Component)))
									{
										resultList.Add(_templateInstance.AddComponent(resultList.GetType().GetGenericArguments()[0]));
									}
									else
									{
										GameObject oGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
										oGo.transform.position = _templateInstance.transform.position;
										oGo.transform.parent = _templateInstance.transform;
										resultList.Add(oGo);
									}

								}
							}
							for (int i = 0; i < _listObjs[propInfo][target]; i++)
							{
								UnityEngine.Object oldRef = (UnityEngine.Object)resultList[i];
								resultList[i] = EditorGUILayout.ObjectField(prop + " - " + i + ":", (UnityEngine.Object)resultList[i], propInfo.PropertyType.GetGenericArguments()[0], true);
								if ((UnityEngine.Object)resultList[i] != oldRef)
								{
									DestroyImmediate(oldRef);
								}
							}
						}
						else if (propInfo.PropertyType.GetGenericArguments()[0].IsPrimitive || propInfo.PropertyType.GetGenericArguments()[0] == typeof(string))
						{
							if (resultList.Count < _listObjs[propInfo][target])
							{
								for (int i = resultList.Count; i < _listObjs[propInfo][target]; i++)
								{
									resultList.Add(Activator.CreateInstance(propInfo.PropertyType.GetGenericArguments()[0]));
								}
							}
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
				else if (result.GetType().GetInterface("IList") != null)
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
					else if (result.GetType().GetInterface("IList") != null)
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

