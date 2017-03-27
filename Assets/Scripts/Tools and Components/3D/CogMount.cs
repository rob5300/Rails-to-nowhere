using UnityEngine;
using System.Collections;

public class CogMount : MonoBehaviour {

    public static Color normalMountColour;

    public Transform MountPosition;
    public CogPuzzle ParentPuzzle;
    public int Size;

    public Cog AttachedCog;
    public bool HasCog = false;

    int emissionID;

    private void Start() {
        if (normalMountColour == null) normalMountColour = GetComponent<MeshRenderer>().material.color;
        emissionID = Shader.PropertyToID("_EmissionColor");
    }

    public void OnAttach(Cog cog) {
        AttachedCog = cog;
        HasCog = true;
    }

    public void OnDetatch() {
        AttachedCog = null;
        HasCog = false;
    }

    public void IncorrectMount() {
        GetComponent<MeshRenderer>().material.color = Color.red;
        GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
        GetComponent<MeshRenderer>().material.SetColor(emissionID, Color.red);
        Invoke("ResetMount", 1f);
    }

    public void CorrectMount() {
        GetComponent<MeshRenderer>().material.color = Color.green;
        GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
        GetComponent<MeshRenderer>().material.SetColor(emissionID, Color.green);
        Invoke("ResetMount", 1f);
    }

    public void ResetMount() {
        GetComponent<MeshRenderer>().material.color = normalMountColour;
        GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
        GetComponent<MeshRenderer>().material.SetColor(emissionID, Color.black);
    }
}
