using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMachineScript : TerminalScript {

    private DoorScript doorScript;

    // Use this for initialization
    void Start() {
        doorScript = GetComponentInParent<DoorScript>();
    }

    // Update is called once per frame
    void Update() {

    }

    protected override void ToggleTerminalFunction(PlayerController player) {
        if (doorScript.IsLocked()) {
            if (player.GetInventory().HasItem(PickupObjectScript.PickupObjectType.KEYCARD)) {
                player.GetInventory().RemoveItem(PickupObjectScript.PickupObjectType.KEYCARD);
                doorScript.Unlock();
            }
        } else {
            player.GetInventory().AddItem(PickupObjectScript.PickupObjectType.KEYCARD);
            doorScript.Lock();
        }
    }

    public override void Toggle() {
        ToggleTerminalFunction(GameManager.GetInstance().player);
    }

    protected override void TurnOn() {

    }

    protected override void TurnOff() {

    }
}
