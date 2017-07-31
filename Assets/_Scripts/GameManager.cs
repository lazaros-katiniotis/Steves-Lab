using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public PlayerController player;
    public Transform currentLevelTransform;
    public Transform canvasTransform;

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
        player.transform.SetParent(null);
        currentLevelTransform.gameObject.SetActive(false);
        canvasTransform.DetachChildren();
    }

    public static GameManager GetInstance() {
        return instance;
    }

    public List<MachineScript> GetMachines() {
        return machines;
    }
}
