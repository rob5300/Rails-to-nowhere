using System.Collections.Generic;
using System.Linq;

public class Inventory {
    private ItemSlot item;
    private List<ItemSlot> itemSlots = new List<ItemSlot>();

    public List<ItemSlot> ItemSlots {
        get {
            return itemSlots;
        }
    }

    public Inventory(int capacity) {
        itemSlots = new List<ItemSlot>(capacity);
        for(int i = 0; i < capacity; i++) {
            itemSlots.Add(new ItemSlot());
        }
    }

    public void AddItem(string id, int amount) {
        ItemSlot itemSlot = GetItemSlot(id);
        if (itemSlot != null) {
            itemSlot.ItemQuantity += amount;
        }
        else {
            foreach (ItemSlot slot in itemSlots) {
                if (slot.ItemID == "" || slot.ItemID == null || slot.ItemID == string.Empty) {
                    slot.ItemID = id;
                    slot.ItemQuantity = amount;
                    break;
                }
            }
        }
    }

    public void RemoveItem(string id, int amount) {
        ItemSlot itemSlot = GetItemSlot(id);
        if (itemSlot != null) {
            itemSlot.ItemQuantity -= amount;
        }
    }

    private ItemSlot GetItemSlot(string itemID) {
        foreach (ItemSlot item in itemSlots) {
            if (item.ItemID == itemID) {
                return item;
            }
        }
        return null;
    }

    public Item GetItem(int SlotID) {
        string id = itemSlots[SlotID].ItemID;
        return Item.GetItem(id);
    }

    public List<Item> GetItems() {
        return itemSlots.Select(x => Item.GetItem(x.ItemID)).ToList();
    }

    public List<ItemSlot> GetPopulatedItemSlots() {
        return itemSlots.Where(x => x.ItemID != null && x.ItemID != string.Empty && x.ItemID != "").ToList();
    }

    public class ItemSlot {
        private string itemID;
        private int itemQuantity;

        public string ItemID {
            get {
                return itemID;
            }

            set {
                itemID = value;
            }
        }

        public int ItemQuantity {
            get {
                return itemQuantity;
            }

            set {
                itemQuantity = value;
                if (itemQuantity < 0) {
                    itemQuantity = 0;
                    ItemID = "";
                }
            }
        }
    }
}


