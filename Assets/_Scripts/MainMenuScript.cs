using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour {

    private string currentLevel;

	// Use this for initialization
	void Start () {
        currentLevel = "test";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayGame() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentLevel);
    }
}
