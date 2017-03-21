using UnityEngine;
using System.Collections.Generic;

public class Item {

    public string ID;
    public string Name;
    public string Description;
    public bool Dropable = true;
    public Sprite InventorySprite;
    public GameObject Prefab;

    public Item(string name, string id, string description, Sprite inventorySprite, GameObject prefab)
    {
        Name = name;
        ID = id;
        Description = description;
        InventorySprite = inventorySprite;
        Prefab = prefab;
        ItemList.Add(id, this);
    }

    public Item(string name, string id, string description) {
        Name = name;
        ID = id;
        Description = description;

        //If a sprite is not supplied, use the unknown item sprite.
        InventorySprite = Resources.Load<Sprite>("ItemIcons/unknown item");

        ////If a prefab is not supplied, use a preset primivite as a placeholder.
        //Prefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //Prefab.SetActive(false);
        //Prefab.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        //Prefab.GetComponent<MeshRenderer>().material.color = Color.magenta;
        //WorldItem newEnt = Prefab.AddComponent<WorldItem>();
        //newEnt.ItemID = id;
        //newEnt.Name = name;
        //newEnt.Quantity = 1;

        ItemList.Add(id, this);
    }

    public Item(string name, string id, string description, Sprite inventorySprite) : this(name, id, description) {
        InventorySprite = inventorySprite;
    }

    public Item(string name, string id, string description, GameObject gameObject) : this(name, id, description) {
        Prefab = gameObject;
    }

    private static Dictionary<string, Item> ItemList = new Dictionary<string, Item>();
    private static Item box = new Item("Box", "item.box", "I am a box that does nothing", Resources.Load<GameObject>("ItemsToPickup/Cube"));
    private static Item battery = new Item("AA Battery", "puzzle.battery", "A double a battery.");
    private static Memory memory3 = new Memory("Memory 3", "memory.3", "The third memory.", Resources.Load<Sprite>("Memories/memory3"));
    private GameObject gameObject;

    public static Item GetItem(string ItemID)
    {
        return ItemList[ItemID];
    }

    public static bool IsValidItemID(string ItemID)
    {
        return ItemList.ContainsKey(ItemID);
    }
}
