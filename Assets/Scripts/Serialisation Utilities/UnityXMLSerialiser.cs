using UnityEngine;
using System.Collections;
using System.Xml.Linq;
using System.Linq;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Collections.Generic;
using System.Reflection;
using System;

/// <summary>
/// Static members are used for non-MonoBehaviour objects. This class can serialise and deserialise targeted objects in MonoBehaviours to XML.
/// </summary>
/// <typeparam name="T">Target type of MonoBehaviour to serialise/deserialise.</typeparam>
public class UnityXMLSerialiser<T> where T : MonoBehaviour, IUnityXMLSerialisable
{
	private GameObject _template;

	public UnityXMLSerialiser()
	{
		_template = GameObject.Find("InstanceHolderObj");
	}

	/// <summary>
	/// Serialises a MonoBehaviour's targeted properties into XML.
	/// </summary>
	/// <param name="target">The target MonoBehaviour to serialise.</param>
	/// <param name="targetInfo">The location of where the resulting file should be saved.</param>
	public void SerialiseInstance(T target, FileInfo targetInfo)
	{
		List<string> targets = target.GetSerialiseTargets();
		XDocument objDoc = new XDocument(new XElement("Inject"));
		using (XmlWriter xw = objDoc.CreateWriter())
		{
			List<PropertyInfo> props = target.GetType().GetProperties().Where(x => targets.Contains(x.Name)).ToList();
			foreach (PropertyInfo prop in props)
			{
				Type propType;
				if (prop.PropertyType.GetInterface("IList") != null)
				{
					if (prop.PropertyType.IsArray)
					{
						propType = prop.PropertyType.GetElementType();
					}
					else
					{
						propType = prop.PropertyType.GetGenericArguments()[0];
					}
				}
				else
				{
					propType = prop.PropertyType;
				}
				if (propType.IsSubclassOf(typeof(UnityEngine.Object)))
				{
					if (prop.PropertyType.GetInterface("IList") != null)
					{
						IList bla = (IList)prop.GetValue(target, null);
						List<string> listOfGameObjectNames = new List<string>();
						foreach (var unityObjItem in bla)
						{
							UnityEngine.Object actualItem = (UnityEngine.Object)unityObjItem;
							string name = actualItem.name;
							listOfGameObjectNames.Add(name);
						}
						XDocument subDoc = new XDocument();
						using (XmlWriter subXW = subDoc.CreateWriter())
						{
							XmlSerializer serialiser = new XmlSerializer(typeof(List<string>), new XmlRootAttribute(prop.Name));
							serialiser.Serialize(subXW, listOfGameObjectNames);
						}
						subDoc.Root.Attributes("xmlns").Remove();
						objDoc.Root.Add(subDoc.Root);
					}
					else
					{
						string name = ((UnityEngine.Object)prop.GetValue(target, null)).name;
						XDocument subDoc = new XDocument();
						using (XmlWriter subXW = subDoc.CreateWriter())
						{
							XmlSerializer serialiser = new XmlSerializer(typeof(string), new XmlRootAttribute(prop.Name));
							serialiser.Serialize(subXW, name);
						}
						subDoc.Root.Attributes("xmlns").Remove();
						objDoc.Root.Add(subDoc.Root);
					}

				}
				else
				{
					XDocument subDoc = new XDocument();
					using (XmlWriter subXW = subDoc.CreateWriter())
					{
						XmlSerializer serialiser = new XmlSerializer(prop.PropertyType, new XmlRootAttribute(prop.Name));
						serialiser.Serialize(subXW, prop.GetValue(target, null));
					}
					subDoc.Root.Attributes("xmlns").Remove();
					objDoc.Root.Add(subDoc.Root);
				}

			}
		}
		objDoc.Save(targetInfo.FullName);
	}

	/// <summary>
	/// Deserialises the target file into it's appropriate MonoBehaviour object instance and puts it on the dummy object for instantiation reasons.
	/// </summary>
	/// <param name="targetInfo">Location of the file to deserialise.</param>
	/// <returns>Target MonoBehaviour type with deserialised values.</returns>
	public T DeserialiseXML(FileInfo targetInfo, bool clearOldInstances = true)
	{
		List<string> derivedPropNames = new List<string>();
		if (_template.GetComponent<T>() != null && clearOldInstances)
		{
			GameObject.Destroy(_template.GetComponent<T>());
		}
		T newObj = _template.AddComponent<T>();
		List<string> targets = new List<string>();
		targets = newObj.GetSerialiseTargets();
		XDocument objDoc = XDocument.Load(targetInfo.FullName);
		List<PropertyInfo> props = typeof(T).GetProperties().Where(x => targets.Contains(x.Name)).ToList();
		foreach (PropertyInfo prop in props)
		{
			if (prop.DeclaringType == typeof(T))
			{
				Type propType;
				if (prop.PropertyType.GetInterface("IList") != null)
				{
					if (prop.PropertyType.IsArray)
					{
						propType = prop.PropertyType.GetElementType();
					}
					else
					{
						propType = prop.PropertyType.GetGenericArguments()[0];
					}
				}
				else
				{
					propType = prop.PropertyType;
				}
				string propFolder = newObj.GetUnityResourcesFolderPath(prop.Name);
				if (propType.IsSubclassOf(typeof(UnityEngine.Object)))
				{
					if (prop.PropertyType.GetInterface("IList") != null)
					{
						using (XmlReader xr = objDoc.Root.Element(prop.Name).CreateReader())
						{
							XmlSerializer propDeserialiser = new XmlSerializer(typeof(List<string>), new XmlRootAttribute(prop.Name));
							List<string> result = (List<string>)propDeserialiser.Deserialize(xr);
							 IList resultList = (IList)Activator.CreateInstance(prop.PropertyType);
							for (int i = 0; i < result.Count; i++)
							{
								string resultPath = result[i];
								if (propFolder != "")
								{
									resultPath = propFolder + "/" + result[i];
								}
								resultList.Add(Resources.Load(resultPath, propType));
							}
							prop.SetValue(newObj, resultList, null);

						}
						targets.Remove(targets.Where(x => x == prop.Name).First());
					}
					else
					{
						using (XmlReader xr = objDoc.Root.Element(prop.Name).CreateReader())
						{
							XmlSerializer propDeserialiser = new XmlSerializer(typeof(string), new XmlRootAttribute(prop.Name));
							string result = (string)propDeserialiser.Deserialize(xr);
							if (propFolder != "")
							{
								result = propFolder + "/" + result;
							}
							prop.SetValue(newObj, Resources.Load(result, prop.PropertyType), null);
						}
						targets.Remove(targets.Where(x => x == prop.Name).First());
					}

				}
				else
				{
					using (XmlReader xr = objDoc.Root.Element(prop.Name).CreateReader())
					{
						XmlSerializer propDeserialiser = new XmlSerializer(prop.PropertyType, new XmlRootAttribute(prop.Name));
						prop.SetValue(newObj, propDeserialiser.Deserialize(xr), null);
					}
					targets.Remove(targets.Where(x => x == prop.Name).First());
				}

			}

		}
		Type baseType = typeof(T).BaseType;
		while (baseType != typeof(MonoBehaviour))
		{
			List<PropertyInfo> inheritedProps = baseType.GetProperties().Where(x => targets.Contains(x.Name)).ToList();
			if (inheritedProps.Any())
			{
				foreach (PropertyInfo prop in inheritedProps)
				{
					Type propType;
					if (prop.PropertyType.GetInterface("IList") != null)
					{
						if (prop.PropertyType.IsArray)
						{
							propType = prop.PropertyType.GetElementType();
						}
						else
						{
							propType = prop.PropertyType.GetGenericArguments()[0];
						}
					}
					else
					{
						propType = prop.PropertyType;
					}
					if (propType.IsSubclassOf(typeof(UnityEngine.Object)))
					{
						string propFolder = newObj.GetUnityResourcesFolderPath(prop.Name);
						if (prop.PropertyType.IsAssignableFrom(typeof(IList)))
						{
							using (XmlReader xr = objDoc.Root.Element(prop.Name).CreateReader())
							{
								XmlSerializer propDeserialiser = new XmlSerializer(typeof(List<string>), new XmlRootAttribute(prop.Name));
								List<string> result = (List<string>)propDeserialiser.Deserialize(xr);
								List<UnityEngine.Object> resultList = new List<UnityEngine.Object>();
								for (int i = 0; i < result.Count; i++)
								{
									string resultPath = result[i];
									if (propFolder != "")
									{
										resultPath = propFolder + "/" + result[i];
									}
									resultList.Add(Resources.Load(resultPath, typeof(UnityEngine.Object)));
								}
								prop.SetValue(newObj, resultList, null);

							}
							targets.Remove(targets.Where(x => x == prop.Name).First());
						}
						else
						{
							using (XmlReader xr = objDoc.Root.Element(prop.Name).CreateReader())
							{
								XmlSerializer propDeserialiser = new XmlSerializer(typeof(string), new XmlRootAttribute(prop.Name));
								string result = (string)propDeserialiser.Deserialize(xr);
								if (propFolder != "")
								{
									result = propFolder + "/" + result;
								}
								prop.SetValue(newObj, Resources.Load(result, prop.PropertyType), null);
							}
							targets.Remove(targets.Where(x => x == prop.Name).First());
						}
					}
					else
					{
						using (XmlReader xr = objDoc.Root.Element(prop.Name).CreateReader())
						{
							XmlSerializer propDeserialiser = new XmlSerializer(prop.PropertyType, new XmlRootAttribute(prop.Name));
							prop.SetValue(newObj, propDeserialiser.Deserialize(xr), null);
						}
						targets.Remove(targets.Where(x => x == prop.Name).First());
					}


				}
			}

			baseType = baseType.BaseType;

		}

		return newObj;
	}
}
