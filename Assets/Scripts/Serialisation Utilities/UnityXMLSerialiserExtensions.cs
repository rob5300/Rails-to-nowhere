using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

public static class UnityXMLSerialiserExtensions
{
	public static void SerialiseSingularNonGameObject<TTargetType>(this TTargetType serialiseTarget, string targetFileLocation)
	{

			using (StreamWriter sw = new StreamWriter(targetFileLocation))
			{
				XmlSerializer serialiser = new XmlSerializer(typeof(TTargetType));
				serialiser.Serialize(sw, serialiseTarget);
			}
		

	}

	public static TTargetType DeserialiseSingularNonGameObject<TTargetType>(this TTargetType objectToLoadInto, string targetFileLocation)
	{
		TTargetType result = default(TTargetType);
		using (StreamReader sr = new StreamReader(targetFileLocation))
		{
			XmlSerializer serialiser = new XmlSerializer(typeof(TTargetType));
			result = (TTargetType)serialiser.Deserialize(sr);
		}
		return result;
	}
}

