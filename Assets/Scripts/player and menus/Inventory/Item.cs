using UnityEngine;
using System.Collections.Generic;

public class Item {

    public string ID;
    public string Name;
    public string Description;
    public Sprite InventorySprite;
    public GameObject Prefab;

    public Item(string name, string id, string description, Sprite img, GameObject prefab)
    {
        Name = name;
        ID = id;
        Description = description;
        InventorySprite = img;
        Prefab = prefab;

        ItemList.Add(id, this);
    }

   
    public static Dictionary<string, Item> ItemList = new Dictionary<string, Item>();

    public static Item box = new Item("Test item", "1", "I am a box that does nothing", Resources.Load<Sprite>("ItemsToPickup/Cubeimage"), Resources.Load<GameObject>("ItemsToPickup/Cube"));

}
