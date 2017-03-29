using UnityEngine;
using System.Collections;

public class Brightness : MonoBehaviour {

    public static float brightness = 1;

	public void SetBrightnessLevel(float level) {
        RenderImage renderi = GetComponent<RenderImage>();
        if (renderi) {
            renderi.brightnessAmount = level;
        }
        else {
            brightness = level;
        }
    }

    void Start() {
        RenderImage renderi = GetComponent<RenderImage>();
        if (renderi) {
            renderi.brightnessAmount = brightness;
        }
    }
}
