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
        if (image.gameObject.activeInHierarchy == false && !UI.MenuOpen)
        {
            UI.MenuOpen = true;
            UI.allowExit = true;
            image.gameObject.SetActive(true);
            Time.timeScale = 0;
            UI.LockPlayerController();
            UI.UnlockCursor();
        }
        else if(image.gameObject.activeInHierarchy == true && UI.allowExit)
        {
            image.gameObject.SetActive(false);
            UI.MenuOpen = false;
            Time.timeScale = 1;
            UI.UnlockPlayerController();
            UI.LockCursor();
        }
    }
}

