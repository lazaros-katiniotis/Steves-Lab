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
            obj.Toggle();
        }

        /*
        //Debug.Log("Machine function togled!");
        foreach (TogglableObject obj in affectedObjects) {
        Type type = obj.GetType();
        if (obj.GetType() == typeof(WallLampScript)) {
        //obj.gameObject.SetActive(!obj.gameObject.activeSelf);
        WallLampScript lamp = obj.GetComponent<WallLampScript>();
        lamp.Toggle();
        } else if (obj.GetType() == typeof(AirVentScript)) {
        AirVentScript vent = obj.GetComponent<AirVentScript>();
        vent.Toggle();
        Debug.Log("Vent: " + vent.transform.name + " is activated:" + vent.IsActivated());
        }
        }
        */
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
