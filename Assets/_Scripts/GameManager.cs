using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {

    public PlayerController player;
    public Transform currentLevelTransform;
    public Transform canvasTransform;
    public TextMeshProUGUI restartMessage;

    private List<MachineScript> machines;

    private static GameManager instance;

    void Start() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
            return;
        }

        InitCurrentLevel();
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

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            AppHelper.Quit();
        }
    }

    public void GameOver() {
        this.transform.SetParent(null);
        //mainCamera.transform.SetParent(this.transform);
        //mainCamera.enabled = true;
        StartCoroutine(DisplayRestartMessage());
        player.transform.SetParent(null);
        player.DisableBars();
        currentLevelTransform.gameObject.SetActive(false);
        canvasTransform.DetachChildren();
    }

    private Color restartMessageColor = new Color(1, 1, 1, 1);

    private IEnumerator DisplayRestartMessage() {
        float elapsed = 0.0f;
        while (elapsed < 2.0f) {
            elapsed += Time.deltaTime;
            yield return null;
        }
        elapsed = 0.0f;
        while (elapsed < 2.0f) {
            restartMessage.color = Color.Lerp(restartMessage.color, restartMessageColor, elapsed);
            yield return null;
        }
        //restartMessage.CrossFadeColor(new Color(1, 1, 1, 0), 2.0f, true, true);
        yield return null;

    }

    public static GameManager GetInstance() {
        return instance;
    }

    public List<MachineScript> GetMachines() {
        return machines;
    }
}
