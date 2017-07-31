using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {

    public PlayerController player;
    public Transform currentLevelTransform;
    public Transform canvasTransform;
    public GameObject gameOverBanner;
    private string currentScene;

    public TextMeshProUGUI restartMessage;
    private bool displayRestartMessage;
    private bool displayGameOverBanner;
    private Color startColor;
    private Color endColor;
    private float messageElapsed = 0.0f;
    private float bannerElapsed = 0.0f;

    private List<MachineScript> machines;
    private static GameManager instance;
    private Vector2 startPos;

    void Start() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
            return;
        }

        currentScene = "test";

        InitCurrentLevel();
    }

    private void Awake() {
        //DontDestroyOnLoad(this.gameObject);
    }

    private void InitCurrentLevel() {
        machines = new List<MachineScript>();
        startColor = restartMessage.color;
        endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f);
        displayRestartMessage = false;
        displayGameOverBanner = false;
    }

    public void AddMachine(DoorScript doorScript) {
        if (doorScript.GetTerminal() != null) {
            machines.Add(doorScript.GetTerminal());
        }
    }

    public void AddMachine(MachineScript machineScript) {
        machines.Add(machineScript);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            AppHelper.Quit();
        }
        if (player.IsDead()) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene);
            }
        }
        if (displayGameOverBanner) {
            bannerElapsed += Time.deltaTime;
            Vector2 finalPos = player.transform.position;
            gameOverBanner.transform.position = Vector2.Lerp(startPos, finalPos, bannerElapsed * bannerElapsed);
        }
        if (displayRestartMessage) {
            messageElapsed += Time.deltaTime;
            restartMessage.color = Color.Lerp(startColor, endColor, messageElapsed * 1.5f);
        }
    }

    public void GameOver() {
        this.transform.SetParent(null);
        //mainCamera.transform.SetParent(this.transform);
        //mainCamera.enabled = true;
        StartCoroutine(GameOverAnimations());
        player.transform.SetParent(null);
        player.DisableBars();
        gameOverBanner.SetActive(true);
        currentLevelTransform.gameObject.SetActive(false);
        //canvasTransform.DetachChildren();
    }

    private IEnumerator GameOverAnimations() {
        float elapsed = 0.0f;
        while (elapsed < 1.7f) {
            if (elapsed > 1.25f) {
                startPos = gameOverBanner.transform.position;
                displayGameOverBanner = true;
            }
            elapsed += Time.deltaTime;
            yield return null;
        }
        displayRestartMessage = true;
        yield return null;

    }

    public static GameManager GetInstance() {
        return instance;
    }

    public List<MachineScript> GetMachines() {
        return machines;
    }
}
