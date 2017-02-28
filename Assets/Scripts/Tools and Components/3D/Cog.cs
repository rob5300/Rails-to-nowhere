using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cog : Entity {

    public int Size;
    public Transform MountPosition;
    public bool Mounted = false;

    CogMount currentMount;

    public void OnDrop() {
        List<RaycastHit> hits = Physics.SphereCastAll(transform.position, 1, Vector3.one).ToList();
        if (hits.Count > 0){
            currentMount = hits.Where(x => x.collider.GetComponent<CogMount>() != null).First().collider.GetComponent<CogMount>();
            if(Vector3.Distance(currentMount.MountPosition.position,MountPosition.position) < .4) {
                Mounted = true;
                GetComponent<Rigidbody>().isKinematic = true;
                transform.position = currentMount.MountPosition.position + transform.InverseTransformPoint(MountPosition.position);
            }
        }
    }

    public void OnPickup() {
        Mounted = false;
        GetComponent<Rigidbody>().isKinematic = false;
    }

}
