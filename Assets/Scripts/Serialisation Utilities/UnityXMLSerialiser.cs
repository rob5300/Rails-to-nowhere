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
				if (prop.PropertyType.IsSubclassOf(typeof(UnityEngine.Object)))
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
		List<PropertyInfo> props = typeof(T).GetType().GetProperties().Where(x => targets.Contains(x.Name)).ToList();
		foreach (PropertyInfo prop in props)
		{
			if (prop.DeclaringType == typeof(T))
			{
				string propFolder = newObj.GetUnityResourcesFolderPath(prop.Name);
				if (prop.PropertyType.IsSubclassOf(typeof(UnityEngine.Object)))
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
					if (prop.PropertyType.IsSubclassOf(typeof(UnityEngine.Object)))
					{
						using (XmlReader xr = objDoc.Root.Element(prop.Name).CreateReader())
						{
							XmlSerializer propDeserialiser = new XmlSerializer(typeof(string), new XmlRootAttribute(prop.Name));
							string result = (string)propDeserialiser.Deserialize(xr);
							string propFolder = newObj.GetUnityResourcesFolderPath(prop.Name);
							if (propFolder != "")
							{
								result = propFolder + "/" + result;
							}
							prop.SetValue(newObj, Resources.Load(result, prop.PropertyType), null);
						}
						targets.Remove(targets.Where(x => x == prop.Name).First());
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
