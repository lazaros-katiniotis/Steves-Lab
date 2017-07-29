using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorMachineScript : MachineScript {

    public override void ToggleMachineFunction() {
        Debug.Log("Machine function togled!");
        foreach (TogglableObject obj in affectedObjects) {
            Type type = obj.GetType();
            if (obj.GetType() == typeof(BlinkingLightScript)) {
                obj.gameObject.SetActive(!obj.gameObject.activeSelf);
            } else if (obj.GetType() == typeof(OxygenVentScript)) {
                OxygenVentScript vent = obj.GetComponent<OxygenVentScript>();
                vent.ToggleVent();
                Debug.Log("Vent: " + vent.transform.name + " is activated:" + vent.IsActivated());
            }
        }
    }

}
