using UnityEngine;
using System.Collections;
using System;
[RequireComponent(typeof(Player))]
public class Pickup : MonoBehaviour {


    void Start()
    {
        Player.interactEvent += OnRaycastHit;
    }

    void Update()
    {

    }

    void OnRaycastHit(GameObject hit)
    {
        if (hit.tag == "pickup")
                {
            Entity objEntity = hit.GetComponent<Entity>();
            if (hit.GetComponent<Entity>() != null)
            {
                Player.player.inventory.AddItem(objEntity.itemID, objEntity.amount);
                Destroy(hit.gameObject);
            }
                }
            }
        }

