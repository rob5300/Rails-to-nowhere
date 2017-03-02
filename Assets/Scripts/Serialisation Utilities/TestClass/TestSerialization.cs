using UnityEngine;
using System.Collections;
using System.Linq.Expressions;
using System;

public class TestSerialization : MonoBehaviour {

	private UnityXMLSerialiser<StoryNPC> _storyNPCSerialiser;
	public GameObject NPC;

	// Use this for initialization
	void Start () {
		_storyNPCSerialiser = new UnityXMLSerialiser<StoryNPC>();
	}

	public void Serialise()
	{
		_storyNPCSerialiser.SerialiseInstance(NPC.GetComponent<StoryNPC>(), new System.IO.FileInfo(Application.streamingAssetsPath + @"\StoryNPC.xml"));

	}

	public void Deserialise()
	{
		StoryNPC npc =	_storyNPCSerialiser.DeserialiseXML(new System.IO.FileInfo(Application.streamingAssetsPath + @"\StoryNPC.xml"));
	}

}
