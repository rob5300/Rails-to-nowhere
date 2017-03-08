using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Linq.Expressions;

public class InterfaceTesting : MonoBehaviour, IUnityXMLSerialisable {

    private List<int> intList = new List<int>();
    private bool testBool = false;
    private Vector3 transformOb = new Vector3(1f, 2f, 3f);
    [SerializeField]
    private DialogueNode node = new DialogueNode("node", "textare");
    private List<GameObject> gameobList = new List<GameObject>();


    public List<int> IntList {
        get {
            return intList;
        }

        set {
            intList = value;
        }
    }

    public bool TestBool {
        get {
            return testBool;
        }

        set {
            testBool = value;
        }
    }

    public Vector3 TransformOb {
        get {
            return transformOb;
        }

        set {
            transformOb = value;
        }
    }

    public List<GameObject> GameobList {
        get {
            return gameobList;
        }

        set {
            gameobList = value;
        }
    }

    public DialogueNode Node {
        get {
            return node;
        }

        set {
            node = value;
        }
    }

    public List<Expression<Func<object, object>>> GetMappings(string propName) {
        return new List<Expression<Func<object, object>>>() { { x => ((DialogueNode)x).Key }, { x => ((DialogueNode)x).Text }  /*delegate(object o) { return ((DialogueNode)o).Key; }, delegate (object o) { return ((DialogueNode)o).Text; }*/ };
    }

    public List<string> GetSerialiseTargets() {
        return new List<string>() { "Node" };
    }
}
