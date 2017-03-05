using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Inventory {

    public List<ItemSlot> ItemSlots = new List<ItemSlot>();

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


