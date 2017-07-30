using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorMachineScript : MachineScript {

    private Light light;

    private void Start() {
        light = GetComponentInChildren<Light>();
    }

    private void Update() {
        base.Update();
        light.enabled = activated;
    }

    public override void ToggleMachineFunction() {
        activated = !activated;
        Debug.Log("Machine function togled!");
        foreach (TogglableObject obj in affectedObjects) {
            Type type = obj.GetType();
            if (obj.GetType() == typeof(WallLampScript)) {
                //obj.gameObject.SetActive(!obj.gameObject.activeSelf);
                WallLampScript lamp = obj.GetComponent<WallLampScript>();
                lamp.ToggleLamp();
            } else if (obj.GetType() == typeof(AirVentScript)) {
                AirVentScript vent = obj.GetComponent<AirVentScript>();
                vent.ToggleVent();
                Debug.Log("Vent: " + vent.transform.name + " is activated:" + vent.IsActivated());
            }
        }
    }

}
