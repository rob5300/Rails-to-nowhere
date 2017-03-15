using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;


public class PauseScript : MonoBehaviour {

    public Transform image;
    public Transform FPlayer;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }
	
    public void Pause()
    {
        if (image.gameObject.activeInHierarchy == false)
        {
            image.gameObject.SetActive(true);
            Time.timeScale = 0;
            UI.LockPlayerController();
            UI.UnlockCursor();
        }
        else
        {
            image.gameObject.SetActive(false);
            Time.timeScale = 1;
            UI.UnlockPlayerController();
            UI.LockCursor();
        }
    }
}

