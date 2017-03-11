using UnityEngine;

[RequireComponent(typeof(Player))]
public class Pickup : MonoBehaviour
{
    void Start()
    {
        Player.InteractEvent += OnRaycastHit;
    }

    void OnRaycastHit(GameObject hit)
    {
        WorldItem objWorldItem = hit.GetComponent<WorldItem>();
        if (hit.GetComponent<WorldItem>() != null) {
            if (objWorldItem.Interactable == true) {
                Player.player.inventory.AddItem(objWorldItem.ItemID, objWorldItem.Quantity);
                Destroy(hit.gameObject);
            }
        }
    }
}


