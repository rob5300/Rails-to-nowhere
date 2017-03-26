using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class NPC : MonoBehaviour, IUnityXMLSerialisable
{
    public delegate void NPCEvent(NPC npc);
    public event NPCEvent NPCDeath;

    [SerializeField]
	private string _name = "New NPC";
	[SerializeField]
	private bool _interactable = true;
	[SerializeField]
	private float _health = 1;
    [SerializeField]
    private string initialDialogueNodeKey;
    [SerializeField]
    private int memoryResponseTotal = 0;
    [SerializeField]
    private string memoryItemKey = "memory.basic";
    [SerializeField]
    private GameObject modelPrefab;

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

    public string InitialDialogueNodeKey {
        get {
            return initialDialogueNodeKey;
        }

        set {
            initialDialogueNodeKey = value;
        }
    }

    public GameObject ModelPrefab {
        get {
            return modelPrefab;
        }

        set {
            modelPrefab = value;
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
        NPCDeath.Invoke(this);
        Destroy(this);
    }

    public void AwardMemory() {
        Player.player.inventory.AddItem(MemoryItemKey, 1);
    }

    public virtual List<string> GetSerialiseTargets()
	{
		List <string> baseProps = new List<string>();
		foreach (PropertyInfo prop in GetType().GetProperties())
		{
			if (prop.DeclaringType == typeof(NPC) || prop.DeclaringType == GetType().BaseType || prop.DeclaringType == GetType())
			{
				baseProps.Add(prop.Name);
			}
		}
		return baseProps;
	}

	public virtual List<Expression<Func<object, object>>> GetMappings(string propName)
	{
		return null;
	}

	public virtual string GetDisplayValue()
	{
		return Name;
	}

	public virtual string GetUnityResourcesFolderPath(string propName)
	{
		if (propName == "ModelPrefab")
		{
			return "NPCPrefabs";
		}
        else if(propName == "Carriage") {
            return "Carriages";
        }
		else if (propName == "CarriageObjects")
		{
			return "NPCCarriageObjectPrefabs";
		}
		return "";
	}
}
