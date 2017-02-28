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
            FPlayer.GetComponent<FirstPersonController>().enabled = false;
            UnlockCursor();
        }
        else
        {
            image.gameObject.SetActive(false);
            Time.timeScale = 1;
            FPlayer.GetComponent<FirstPersonController>().enabled = true;
            LockCursor();
        }
    }

    public static void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public static void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}

