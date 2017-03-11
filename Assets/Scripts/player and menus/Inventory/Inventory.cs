using System.Collections.Generic;
using System.Linq;

public class Inventory {
    private ItemSlot item;
    private List<ItemSlot> ItemSlots = new List<ItemSlot>();

    public Inventory(int capacity)
    {
        ItemSlots = new List<ItemSlot>(capacity);
        ItemSlots.AddRange(Enumerable.Repeat(new ItemSlot(), capacity));
    }

   public void AddItem(string id, int amount)
    {
        ItemSlot itemSlot = GetItemSlot(id);
        if (itemSlot != null)
        {
            itemSlot.ItemQuantity += amount;
        }
        else
        {
            foreach (ItemSlot slot in ItemSlots)
            {
                if (slot.ItemID == "" || slot.ItemID == null || slot.ItemID == string.Empty)
                {
                    slot.ItemID = id;
                    slot.ItemQuantity = amount;
                    break;
                }
            }
        }
    }

   public void RemoveItem(string id, int amount)
    {
        ItemSlot itemSlot = GetItemSlot(id);
        if (itemSlot != null)
        {
            itemSlot.ItemQuantity -= amount;
        }
    }

    private ItemSlot GetItemSlot(string itemID)
    {
        foreach (ItemSlot item in ItemSlots)
        {
            if (item.ItemID == itemID)
            {
                return item;
            }
        }
        return null;
    }

    public Item GetItem(int SlotID)
    {
        string id = ItemSlots[SlotID].ItemID;
        return Item.GetItem(id);
    }



    private class ItemSlot
    {
        private string itemID;
        private int itemQuantity;

        public string ItemID
        {
            get
            {
                return itemID;
            }

            set
            {
                itemID = value;
            }
        }

        public int ItemQuantity
        {
            get
            {
                return itemQuantity;
            }

            set
            {
                itemQuantity = value;
                if(itemQuantity < 0)
                {
                    itemQuantity = 0;
                    ItemID = "";
                }
            }
        }
    }
}


