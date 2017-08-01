using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorMachineScript : MachineScript {

    private Light light;

    void Start() {
        light = GetComponentInChildren<Light>();
    }

    void Update() {
        base.Update();
        light.enabled = activated;
    }

    protected override void ToggleMachineFunction(PlayerController player) {
        activated = !activated;
        foreach (TogglableObject obj in affectedObjects) {
            Debug.Log("Toggling: " + obj.transform.name);
            obj.Toggle();
        }
    }

    public override void Toggle() {
        if (firstTimeActivated) {
            TurnOn();
            firstTimeActivated = false;
        } else {
            ToggleMachineFunction(GameManager.GetInstance().player);
        }
    }

    public override void TurnOn() {
        activated = true;
        foreach (TogglableObject obj in affectedObjects) {
            Debug.Log(obj.transform.name);
            if (obj.GetType().Equals(typeof(DoorScript))) {
                ((DoorScript)obj).Unlock();
            }
            obj.TurnOn();
        }
    }

    public override void TurnOff() {
        activated = false;
        foreach (TogglableObject obj in affectedObjects) {
            obj.TurnOff();
        }
    }
}
