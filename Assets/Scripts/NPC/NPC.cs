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
}
