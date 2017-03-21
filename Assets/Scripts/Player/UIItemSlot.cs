using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItemSlot : MonoBehaviour, IPointerClickHandler{

    public Image ItemSprite;
    public Text ItemName;
    public Text ItemType;
    public Text ItemQuantity;
    public Item item;

    public void OnPointerClick(PointerEventData eventData) {
        UI.InventoryItemClickEvent(this);
    }
}
