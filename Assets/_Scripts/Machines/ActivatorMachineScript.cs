using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorMachineScript : MachineScript {

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public override void ToggleMachineFunction() {
        Debug.Log("Machine function togled!");
        foreach (GameObject obj in affectedObjects) {
            obj.SetActive(!obj.activeSelf);
        }
    }

}
