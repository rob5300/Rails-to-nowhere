using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public abstract class NPC : MonoBehaviour {

    public string Name = "New NPC";
    public bool Interactable = true;
    public float Health = 1;

    public abstract void Interact();

    public void OnTaskComplete() {

    }

    public void OnTaskBegin() {

    }

    public void OnTaskFail() {

    }

    public virtual void Damage(float damage) {
        if (Health - damage <= 0) {
            Die();
        }
        else {
            Health -= damage;
        }
    }

    public void Die() {
        Destroy(this);
    }
}
