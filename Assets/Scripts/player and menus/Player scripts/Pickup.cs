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
            WorldItem objWorldItem = hit.GetComponent<WorldItem>();
            if (hit.GetComponent<WorldItem>() != null)
            {
                if (objWorldItem.Interactable == true)
                {
                    Player.player.inventory.AddItem(objWorldItem.ItemID, objWorldItem.Quantity);
                    Destroy(hit.gameObject);
                }
            }
                }
            }
        

