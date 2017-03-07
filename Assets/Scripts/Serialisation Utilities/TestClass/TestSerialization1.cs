using UnityEngine;
using System.Collections;
using System.Linq.Expressions;
using System;

public class TestSerialization1 : MonoBehaviour {

	private UnityXMLSerialiser<InterfaceTesting> _serialiser;
	public GameObject testob;

	// Use this for initialization
	void Start () {
		_serialiser = new UnityXMLSerialiser<InterfaceTesting>();
	}

	public void Serialise()
	{
		_serialiser.SerialiseInstance(testob.GetComponent<InterfaceTesting>(), new System.IO.FileInfo(Application.streamingAssetsPath + @"\InterfaceTesting.xml"));

	}

	public void Deserialise()
	{
        InterfaceTesting deser = _serialiser.DeserialiseXML(new System.IO.FileInfo(Application.streamingAssetsPath + @"\InterfaceTesting.xml"));
	}

}
