using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq.Expressions;

public interface IUnityXMLSerialisable
{
	List<string> GetSerialiseTargets();
	List<Func<object, object>> GetMappings(string propName);
}
