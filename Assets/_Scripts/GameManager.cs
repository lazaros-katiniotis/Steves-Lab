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
    private Color startColor;
    private Color endColor;
    private float elapsed = 0.0f;

    private List<MachineScript> machines;

    private static GameManager instance;

    void Start() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
            return;
        }

        currentScene = "test";

        startColor = restartMessage.color;
        endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f);
        InitCurrentLevel();
    }

    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    private void InitCurrentLevel() {
        machines = new List<MachineScript>();
    }

    public void AddMachine(DoorScript doorScript) {
        if (doorScript.GetTerminal() != null) {
            machines.Add(doorScript.GetTerminal());
        }
    }

    public void AddMachine(MachineScript machineScript) {
        machines.Add(machineScript);
    }

    private Vector2 startPos;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            AppHelper.Quit();
        }
        if (player.IsDead()) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene);
            }
        }
        if (displayRestartMessage) {
            elapsed += Time.deltaTime;
            Vector2 finalPos;
            finalPos.x = gameOverBanner.transform.position.x;
            finalPos.y = gameOverBanner.transform.position.y - 6;
            gameOverBanner.transform.position = Vector2.Lerp(startPos, finalPos, elapsed * elapsed);
            restartMessage.color = Color.Lerp(startColor, endColor, elapsed * 1.5f);
        }
    }

    public void GameOver() {
        this.transform.SetParent(null);
        //mainCamera.transform.SetParent(this.transform);
        //mainCamera.enabled = true;
        StartCoroutine(GameOverAnimations());
        player.transform.SetParent(null);
        player.DisableBars();
        currentLevelTransform.gameObject.SetActive(false);
        //canvasTransform.DetachChildren();
    }

    private IEnumerator GameOverAnimations() {
        float elapsed = 0.0f;
        while (elapsed < 1.5f) {
            elapsed += Time.deltaTime;
            yield return null;
        }
        startPos = gameOverBanner.transform.position;
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
