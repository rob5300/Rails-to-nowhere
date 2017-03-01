using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {

    public string Name = "Entity";
    public bool Interactable = true;

    public virtual void OnInteract() {

    }

    void Reset() {
        //Applies a value when first added to a component.
        Name = name;
    }
}
