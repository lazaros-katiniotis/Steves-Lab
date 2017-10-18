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
    public TextMeshProUGUI restartMessage;
    public TextMeshProUGUI keycardText;

    private bool displayRestartMessage;
    private bool displayGameOverBanner;
    private Color startColor;
    private Color endColor;
    private float messageElapsed = 0.0f;
    private float bannerElapsed = 0.0f;

    private List<BoxScript> boxes;
    private static GameManager instance;
    private Vector2 startPos;

    void Start() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
            return;
        }
        InitCurrentLevel();
    }

    private void Awake() {
        //DontDestroyOnLoad(this.gameObject);
    }

    private void InitCurrentLevel() {
        startColor = restartMessage.color;
        endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f);
        displayRestartMessage = false;
        displayGameOverBanner = false;
        GetBoxes();
    }

    private void GetBoxes() {
        boxes = new List<BoxScript>();
        foreach (Transform room in currentLevelTransform.GetComponent<LevelScript>().roomsTransform) {
            foreach (Transform obj in room.Find("OBJECTS")) {
                Debug.Log(obj + ", " + obj.position);
                BoxScript box = obj.GetComponent<BoxScript>();
                if (box != null) {
                    boxes.Add(box);
                }
            }
        }
    }

    public float GetNearestBox(out BoxScript nearestBox) {
        nearestBox = null;
        float minDistance = 100;
        foreach (BoxScript box in boxes) {
            Debug.Log("player pos: " + player.transform.position);
            Debug.Log("box pos: " + box.transform.position);
            Vector3 delta = player.transform.position - box.transform.position;
            float distance = Mathf.Sqrt(delta.x * delta.x + delta.y * delta.y);
            if (distance <= minDistance) {
                minDistance = distance;
                nearestBox = box;
            }
        }
        return minDistance;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            AppHelper.Quit();
        }
        if (player.IsDead()) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                UnityEngine.SceneManagement.SceneManager.LoadScene(DataManager.GetInstance().GetCurrentScene());
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
        keycardText.gameObject.SetActive(false);
        currentLevelTransform.gameObject.SetActive(false);

        //canvasTransform.DetachChildren();
    }

    private IEnumerator GameOverAnimations() {
        float elapsed = 0.0f;
        while (elapsed < 1.5f) {
            if (elapsed > 0.75f) {
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

    //public List<TerminalScript> GetTerminals() {
    //    return terminals;
    //}
}
