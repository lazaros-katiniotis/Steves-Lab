using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour {

    public List<string> scenes;

    private int levelIndex = 0;

    public static DataManager instance;

    private bool gameJustStarted = true;

    public AudioClip intro;
    public AudioClip loop;

    private AudioClip current;
    private AudioSource audio;
    private bool isAudioLooping;

    // Use this for initialization
    void Start() {
        Debug.Log("DataManager Start()...");
        if (instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
            return;
        }

        current = intro;
        audio = GetComponent<AudioSource>();
        audio.clip = current;
        audio.Play();
        audio.loop = false;
        isAudioLooping = false;
    }

    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update() {
        if (!audio.isPlaying && !isAudioLooping) {
            Debug.Log("in here.");
            audio.clip = loop;
            audio.loop = true;
            audio.Play();
            isAudioLooping = true;
        }
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
