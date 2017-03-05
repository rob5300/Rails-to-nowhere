using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Item {

    public string itemID;
    public string itemName;
    public string itemInfo;
    public Sprite itemImg;
    public GameObject itemPrefab;

    public Item(string name, string id, string info, Sprite img, GameObject prefab)
    {
        itemName = name;
        itemID = id;
        itemInfo = info;
        itemImg = img;
        itemPrefab = prefab;

        ItemList.Add(id, this);
    }

   
    public static Dictionary<string, Item> ItemList = new Dictionary<string, Item>();

    public static Item box = new Item("Test item", "1", "I am a box that does nothing", Resources.Load<Sprite>("ItemsToPickup/Cubeimage"), Resources.Load<GameObject>("ItemsToPickup/Cube"));

}
