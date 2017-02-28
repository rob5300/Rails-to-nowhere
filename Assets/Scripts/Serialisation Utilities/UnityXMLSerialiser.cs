using UnityEngine;
using System.Collections;
using System.Xml.Linq;
using System.Linq;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Collections.Generic;
using System.Reflection;

public class UnityXMLSerialiser<T> where T : IUnityXMLSerialisable
{
	public static void SerialiseSingularObject<TTargetType>(TTargetType serialiseTarget, string targetFileLocation)
	{
		if (!Directory.Exists(targetFileLocation))
		{
			File.Create(targetFileLocation);
		}
		using (StreamWriter sw = new StreamWriter(targetFileLocation))
		{
			XmlSerializer serialiser = new XmlSerializer(typeof(TTargetType));
			serialiser.Serialize(sw, serialiseTarget);
		}
	} 

	public void SerialiseInstance(T target, FileInfo targetInfo)
	{
		List<string> targets = target.GetSerialiseTargets();
		XDocument objDoc = new XDocument(new XElement("Inject"));
		using (XmlWriter xw = objDoc.CreateWriter())
		{
			List<PropertyInfo> props = target.GetType().GetProperties().Where(x => targets.Contains(x.Name)).ToList();
			foreach (PropertyInfo prop in props)
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
		objDoc.Save(targetInfo.FullName);
	}
}
