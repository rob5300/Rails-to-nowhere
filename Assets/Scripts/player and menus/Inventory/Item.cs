using UnityEngine;
using System.Collections.Generic;

public class Item {

    public string ID;
    public string Name;
    public string Description;
    public bool Dropable = true;
    public Sprite InventorySprite;
    public GameObject Prefab;

    public Item(string name, string id, string description, Sprite inventorySprite, GameObject prefab) : this(name, id, description) {
        InventorySprite = inventorySprite;
        Prefab = prefab;
    }

    //Base Constructor
    public Item(string name, string id, string description) {
        Name = name;
        ID = id.ToLower();
        Description = description;
        //If a sprite is not supplied, use the unknown item sprite.
        InventorySprite = Resources.Load<Sprite>("ItemIcons/unknown item");

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
	private static Item solderWire = new Item("Solder Wire Coil", "puzzle.solderwire", "Used in fixing electronic components.", Resources.Load<GameObject>("2D Puzzle World objects/Solder Wire Coil"));
	private static Item battery = new Item("AA Battery", "puzzle.battery", "Used to provide power to electronic tools.", Resources.Load<GameObject>("2D Puzzle World objects/Battery"));
    private static Memory memoryMatilda = new Memory("Matilda's Memory", "memory.matilda", "A memory from Matilda.", Resources.Load<Sprite>("Memories/memoryMatilda"));
    private static Memory memoryArleana = new Memory("Arleana's Memory", "memory.arleana", "A memory from Arleana.", Resources.Load<Sprite>("Memories/memoryArleana"));
    private static Memory memoryJohn = new Memory("John's Memory", "memory.john", "A memory from John.", Resources.Load<Sprite>("Memories/memoryJohn"));
    private static Memory memoryNadia = new Memory("Nadia's Memory", "memory.nadia", "A memory from Nadia.", Resources.Load<Sprite>("Memories/memoryNadia"));

	public static Item GetItem(string ItemID)
    {
        if (!ItemList.ContainsKey(ItemID)) {
            Debug.LogError("The ItemID: " + ItemID + " does not exist.");
            return null;
        }
        return ItemList[ItemID.ToLower()];
    }

    public virtual void OnAddToInventory() {

    }

    public static bool IsValidItemID(string ItemID)
    {
        return ItemList.ContainsKey(ItemID);
    }
}
