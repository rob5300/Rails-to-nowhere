using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour {

    public void ChangeMyScene(string myScene) //when void is added, it's not going to return anything, just do something
    {
		Time.timeScale = 1f;
        SceneManager.LoadScene(myScene);

        //a function which is gonna expect a string , which is going to be the name of another scene, and it's going to load that scene
    }
	
	
}
