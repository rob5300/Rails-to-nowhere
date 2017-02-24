using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VolumeChange : MonoBehaviour
{

    public Slider VolumeSlider;
    float masterVolume = 0.0f;

    public void AdjustVolume(float newVolume)
    {
        AudioListener.volume = newVolume;
    }
}

