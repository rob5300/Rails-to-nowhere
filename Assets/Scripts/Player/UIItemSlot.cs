using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItemSlot : MonoBehaviour, ISelectHandler{

    public Image ItemSprite;
    public Text ItemName;
    public Text ItemType;
    public Text ItemQuantity;

    public void OnSelect(BaseEventData eventData){
        UI.InventoryItemClickEvent(this);
    }

}
