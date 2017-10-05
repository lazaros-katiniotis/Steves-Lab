using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour {

    public List<string> scenes;
    public static DataManager instance;

    public AudioClip song;
    private AudioSource audio;
    private float songDelay;

    private int levelIndex = 0;
    private bool gameJustStarted = true;


    // Use this for initialization
    void Start() {
        Debug.Log("DataManager Start()...");
        if (instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
            return;
        }

        audio = GetComponent<AudioSource>();
        audio.clip = song;
        audio.loop = true;
        TurnOnMusic(3.45f);
    }

    public void TurnOnMusic(float delay) {
        audio.volume = 0.65f;
        audio.PlayDelayed(delay);
    }

    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    void Update() {

    }

    public static DataManager GetInstance() {
        return instance;
    }

    public string GetCurrentScene() {
        return scenes[levelIndex];
    }

    public void NextLevel() {
        levelIndex++;
        UnityEngine.SceneManagement.SceneManager.LoadScene(GetCurrentScene());
    }

    public bool HasGameJustStarted() {
        return gameJustStarted;
    }

    public void SetGameJustStarted(bool value) {
        gameJustStarted = value;
    }
}
