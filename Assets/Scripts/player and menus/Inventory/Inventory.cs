using System.Collections.Generic;
using System.Linq;

public class Inventory {

    private List<ItemSlot> ItemSlots = new List<ItemSlot>();

    public Inventory(int capacity)
    {
        ItemSlots = new List<ItemSlot>(capacity);
        ItemSlots.AddRange(Enumerable.Repeat(new ItemSlot(), capacity));
    }

    void AddItem()
    {

    }

    void RemoveItem()
    {

    }

    private class ItemSlot
    {
        public string itemID;
        public int itemQuantity;
    }
}


