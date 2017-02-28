using UnityEngine;
using System.Collections;

public class TestSerialization : MonoBehaviour {

	private UnityXMLSerialiser<StoryNPC> _storyNPCSerialiser;
	public GameObject NPC;

	// Use this for initialization
	void Start () {
		_storyNPCSerialiser = new UnityXMLSerialiser<StoryNPC>();
	}

	public void Serialise()
	{
		print("fired");
		_storyNPCSerialiser.SerialiseInstance(NPC.GetComponent<StoryNPC>(), new System.IO.FileInfo("test.xml"));

	}

}
