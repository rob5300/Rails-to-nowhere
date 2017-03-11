using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Entities/Leaver", 1)]
[RequireComponent(typeof(Collider))]
public class Lever : Entity {

    public UnityEvent OnInteractEvent;
    //public Animator animator;

    public override void OnInteract() {
        OnInteractEvent.Invoke();
    }

}
