using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq.Expressions;

public interface IUnityXMLSerialisable
{
	List<string> GetSerialiseTargets();
	List<Expression<Func<object, object>>> GetMappings(string propName);
	string GetDisplayValue();
	string GetUnityResourcesFolderPath(string propName);
}
