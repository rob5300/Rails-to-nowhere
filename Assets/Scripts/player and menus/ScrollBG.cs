using UnityEngine;
using System.Collections;

public class ScrollBG : MonoBehaviour
{
    public float speed = 0.5f;
    Vector2 offset = new Vector2(0, 0);

    void Update()
    {
        offset.x += Time.deltaTime * speed;
        if (offset.x >= 1 || offset.x <= -1) {
            offset = new Vector2(0, 0);
        }
        GetComponent<Renderer>().material.mainTextureOffset = offset;
    }

}

