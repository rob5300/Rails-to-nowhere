using UnityEngine;
using System.Collections;

[AddComponentMenu("Entities/Entity", 0)]
[RequireComponent(typeof(Collider))]
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
