using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cog : Entity {

    public int Size;
    public Transform MountPoint;
    public bool Mounted = false;

    public void OnDrop() {
        List<RaycastHit> hits = Physics.SphereCastAll(transform.position, 3, Vector3.zero).ToList();
        CogMount mount = hits.Where(x => x.collider.GetComponent<CogMount>() != null).First().collider.GetComponent<CogMount>();

    }

}
