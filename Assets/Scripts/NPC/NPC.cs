using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public abstract class NPC : MonoBehaviour, IUnityXMLSerialisable
{

	[SerializeField]
	private string _name = "New NPC";
	[SerializeField]
	private bool _interactable = true;
	[SerializeField]
	private float _health = 1;
    private string initialDialogueNodeName;

    private int memoryResponseTotal = 0;
    private string memoryItemKey = "memory.basic";

    public string Name
	{
		get
		{
			return _name;
		}

		set
		{
			_name = value;
		}
	}

	public bool Interactable
	{
		get
		{
			return _interactable;
		}

		set
		{
			_interactable = value;
		}
	}

	public float Health
	{
		get
		{
			return _health;
		}

		set
		{
			_health = value;
		}
	}

    public int MemoryResponseTotal {
        get {
            return memoryResponseTotal;
        }

        set {
            memoryResponseTotal = value;
        }
    }

    public string MemoryItemKey {
        get {
            return memoryItemKey;
        }

        set {
            memoryItemKey = value;
        }
    }

    public string InitialDialogueNodeName {
        get {
            return initialDialogueNodeName;
        }

        set {
            initialDialogueNodeName = value;
        }
    }

    public virtual void Interact() {

    }

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

    public void AwardMemory() {
        Player.player.inventory.AddItem(MemoryItemKey, 1);
    }

    public virtual List<string> GetSerialiseTargets()
	{
		List <string> baseProps = new List<string>();
		baseProps.Add("Name");
		baseProps.Add("Interactable");
		baseProps.Add("Health");
		return baseProps;
	}

	public virtual List<Expression<Func<object, object>>> GetMappings(string propName)
	{
		return null;
	}

	public virtual string GetDeleteDisplayProp()
	{
		return Name;
	}
}
