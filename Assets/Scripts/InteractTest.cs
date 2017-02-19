using UnityEngine;
using UnityEngine.UI;

public class InteractTest : MonoBehaviour {

    public float ReachDistance;
    public float velocityRatio = 0f;
    public Text debugVel;
    public float VelocityClamp = 10;

    bool _haveGrabbedObject = false;
    Vector3 raycastStart;
    Vector3 fowardDirection;
    RaycastHit grabbedHit;
    Vector3 offset;
    Vector3 newMovePoint;

    GameObject grabbedObject;

    void Update() {
        //--We have to recalculate these each frame, as the camera moves all the time--
        //The position to start the raycast from, this position is the perfect center of the screen.
        raycastStart = Camera.main.transform.position;
        //The direction for the raycayst to go, foward from the camera.
        fowardDirection = Camera.main.transform.TransformDirection(Vector3.forward);

        //We check if the right mouse button is being held down AND we are not already holding an object.
        if (Input.GetMouseButton(1) && !_haveGrabbedObject) {
            //We perform a raycast to find an object in front of us. Physics.Raycast returns true if it has hit an object.
            if (Physics.Raycast(raycastStart, fowardDirection, out grabbedHit, ReachDistance)) {
                //We have hit an object. Check if it has a rigidbody.
                if (grabbedHit.collider.GetComponent<Rigidbody>() != null) {
                    //Record that we are now carrying an object.
                    _haveGrabbedObject = true;
                    //Store the gameobject for later use.
                    grabbedObject = grabbedHit.collider.gameObject;
                    //Record the initial offset from the object to the camera. Means that the new position that we move the object to will be the same distance away from the camera always.
                    offset = Camera.main.transform.InverseTransformPoint(grabbedObject.transform.position);
                }
            }
        }
        //If we release right click and have an object, we drop it.
        else if (_haveGrabbedObject && !Input.GetMouseButton(1)) {
            _haveGrabbedObject = false;
            grabbedObject = null;
        }
    }

    void FixedUpdate() {
        //We manage moving the object here. Physics tasks should always be done in FixedUpdate.
        if (_haveGrabbedObject) {
            //If we have a grabbed object, do this.
            //Calculate the new point to move towards.
            newMovePoint = Camera.main.transform.TransformPoint(offset);
            //Store this new calculated velocity for the object.
            Vector3 newVelocity = (newMovePoint - grabbedObject.transform.position) * velocityRatio;
            //We then perform a clamp on the velocity to make sure it doesnt go overly high and cause the object to snap through other objects.
            newVelocity = new Vector3(Mathf.Clamp(newVelocity.x,-VelocityClamp, VelocityClamp), Mathf.Clamp(newVelocity.y, -VelocityClamp, VelocityClamp), Mathf.Clamp(newVelocity.z, -VelocityClamp, VelocityClamp));
            //Apply this velocity.
            grabbedObject.GetComponent<Rigidbody>().velocity = newVelocity;
            //Show the object velocity for debugging purposes.
            debugVel.text = "Velocity: " + grabbedObject.GetComponent<Rigidbody>().velocity;
        }
    }
}
