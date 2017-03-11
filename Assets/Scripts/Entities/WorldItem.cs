using UnityEngine;
using System.Collections;

[AddComponentMenu("Entities/World Item", 2)]
[RequireComponent(typeof(Collider))]
public class WorldItem : Entity {

    public string ItemID = "";
    public int Quantity = 1;

	public override void OnInteract() {

    }

}
