using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio; //allsows access to classes like audio mixer
using System.Collections;

public class VolumeChange : MonoBehaviour
{
    public AudioMixer masterMixer;
    
    public void SetSFXLvl(float sfxLvl)
    {
        masterMixer.SetFloat("SFXvol", sfxLvl); //allows manipulation of the child mixer sfx
    }

    public void SetMusicLvl( float musicLvl)
    {
        masterMixer.SetFloat("MusicVol", musicLvl); //same as above but for music
    }

    public void SetMasterLvl(float masterLvl )
    {
        masterMixer.SetFloat("masterVolume", masterLvl);
    }
}

