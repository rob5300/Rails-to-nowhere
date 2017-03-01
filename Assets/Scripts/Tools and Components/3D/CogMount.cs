using UnityEngine;
using System.Collections;

public class CogMount : MonoBehaviour {

    public Transform MountPosition;
    public CogPuzzle ParentPuzzle;
    public int Size;

    public Cog AttachedCog;
    public bool HasCog = false;

    public void OnAttach(Cog cog) {
        AttachedCog = cog;
        HasCog = true;
    }

    public void OnDetatch() {
        AttachedCog = null;
        HasCog = false;
    }

}
