using UnityEngine;
using UnityEngine.Events;

public class Lever : Entity {

    public UnityEvent OnInteractEvent;
    //public Animator animator;

    public override void OnInteract() {
        OnInteractEvent.Invoke();
    }

}
