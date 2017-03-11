using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Entity), true)]
public class EntityInspector : Editor {

    Sprite missingSprite;

    public override void OnInspectorGUI() {
        Entity entity = (Entity)target;
        WorldItem wItem = null;
        if (entity is WorldItem)
            wItem = (WorldItem)entity;
        //Name
        entity.Name = EditorGUILayout.TextField("Name", entity.Name);

        //Interactable Toggle
        entity.Interactable = EditorGUILayout.Toggle("Interactable", entity.Interactable);

        if (wItem != null) {
            //Item id for the item this entity represents
            wItem.ItemID = EditorGUILayout.TextField("Item ID", wItem.ItemID);

            //Quantity Slider
            if (wItem.Interactable)
                wItem.Quantity = EditorGUILayout.IntSlider("Quantity", wItem.Quantity, 1, 64);

            //Here we check if the id is correct, and display the information for the item that corrisponds to that id:
            EditorGUILayout.BeginVertical(EditorStyles.textArea);
            if (wItem.ItemID != null || wItem.ItemID != "") {
                string itemid = wItem.ItemID;
                if (Item.IsValidItemID(itemid)) {
                    EditorGUILayout.LabelField("Item information:", EditorStyles.boldLabel);
                    Item founditem = Item.GetItem(wItem.ItemID);
                    EditorGUILayout.LabelField("Item Name: ", founditem.Name);
                    EditorGUILayout.LabelField("Description: ", founditem.Description);

                    //Icon
                    EditorGUILayout.LabelField("Item Icon: ");
                    if (founditem.InventorySprite != null) {
                        Rect rect = GUILayoutUtility.GetRect(100, 100);
                        EditorGUI.DrawTextureTransparent(rect, founditem.InventorySprite.texture, ScaleMode.ScaleToFit);
                    }
                    else {
                        Rect rect = GUILayoutUtility.GetRect(100, 100);
                        if (missingSprite == null) missingSprite = Resources.Load<Sprite>("ItemIcons/unknown item");
                        EditorGUI.DrawTextureTransparent(rect, missingSprite.texture, ScaleMode.ScaleToFit);
                    }
                }
                else {
                    EditorGUILayout.LabelField("Error, " + wItem.ItemID + " is invalid!", EditorStyles.boldLabel);
                    EditorGUILayout.LabelField("Did you forget to define the Item?", EditorStyles.boldLabel);
                }
            }
            else {
                EditorGUILayout.LabelField("Error, itemid is null!", EditorStyles.boldLabel);
            }
            EditorGUILayout.EndVertical(); 
        }
        //Can be used later if another entity type that needs this comes along.

        //else if (entity.Type == Entity.EntityType.WorldObject) {
        //    //Draw things for a world object.

        //    //Name
        //    entity.Name = EditorGUILayout.TextField("Name", entity.Name);

        //    EditorGUILayout.BeginHorizontal();
        //    //Description Box
        //    EditorGUILayout.PrefixLabel("Description");
        //    entity.Description = EditorGUILayout.TextArea(entity.Description);
        //    EditorGUILayout.EndHorizontal();

        //    //Interactable Toggle
        //    entity.Interactable = EditorGUILayout.Toggle("Interactable", entity.Interactable);
        //}
    }

}
