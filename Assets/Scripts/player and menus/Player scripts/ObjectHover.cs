using UnityEngine;
using UnityEngine.UI;

public class ObjectHover : MonoBehaviour
{
    Vector3 raycastStart;
    Vector3 fowardDirection;
    RaycastHit hit;
    WorldItem item;

    private float ReachDistance;
    private Text HoverName;
    private Text HoverDescription;
    private bool _wasShowing = false;

    void Start()
    {
        if (UI.ui == null) {
            Debug.LogError("No UI found. Required for hover functionality. Object hover is now disabled.");
            GetComponent<ObjectHover>().enabled = false;
        }

        ReachDistance = Player.player.GetComponent<EntityInteract>().ReachDistance;
        HoverName = UI.ui.HoverName;
        HoverDescription = UI.ui.HoverDescription;

        HoverName.gameObject.SetActive(false);
        HoverDescription.gameObject.SetActive(false);
    }

    void Update()
    {
        raycastStart = Camera.main.transform.position;
        fowardDirection = Camera.main.transform.TransformDirection(Vector3.forward);
        item = null;

        if (Physics.Raycast(raycastStart, fowardDirection, out hit, ReachDistance)) {
            item = hit.collider.GetComponent<WorldItem>();
            if (item) {
                HoverName.gameObject.SetActive(true);
                HoverDescription.gameObject.SetActive(true);
                HoverDescription.text = Item.GetItem(item.ItemID).Description;
                HoverName.text = item.Name;
                _wasShowing = true;
            }
            else if (_wasShowing) {
                HoverName.gameObject.SetActive(false);
                HoverDescription.gameObject.SetActive(false);
                _wasShowing = false;
            }
        }
        else if(_wasShowing) {
            HoverName.gameObject.SetActive(false);
            HoverDescription.gameObject.SetActive(false);
            _wasShowing = false;
        }
    }
}

