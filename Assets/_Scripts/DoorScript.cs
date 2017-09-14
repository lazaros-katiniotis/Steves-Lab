using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : TogglableObject {

    public Transform closedSpriteTransform;
    public Transform openSpriteTransform;
    public Transform closedIndicatorTransform;
    public Transform openIndicatorTransform;

    public RoomScript previousRoom;
    public RoomScript nextRoom;

    private bool locked;

    private void Awake() {
        locked = activeOnStart;
        Toggle(null);
        TurnOff();
    }

    public override void Toggle(Actor actor) {
        if (locked) {
            if (actor != null) {
                actor.GetInventory().RemoveItem(PickupObjectScript.PickupObjectType.KEYCARD);
            }
            locked = false;
            closedIndicatorTransform.gameObject.SetActive(false);
            openIndicatorTransform.gameObject.SetActive(true);
            TurnOn();
        } else {
            if (actor != null) {
                actor.GetInventory().AddItem(PickupObjectScript.PickupObjectType.KEYCARD);
            }
            locked = true;
            closedIndicatorTransform.gameObject.SetActive(true);
            openIndicatorTransform.gameObject.SetActive(false);
            TurnOff();
        }
    }

    public override void TurnOn() {
        if (!locked) {
            closedSpriteTransform.gameObject.SetActive(false);
            openSpriteTransform.gameObject.SetActive(true);
        }
    }

    public override void TurnOff() {
        closedSpriteTransform.gameObject.SetActive(true);
        openSpriteTransform.gameObject.SetActive(false);
    }

    public override Component GetParticleColliderTransform() {
        throw new NotImplementedException();
    }

    public RoomScript GetNextRoom() {
        return nextRoom;
    }


    public RoomScript GetPreviousRoom() {
        return previousRoom;
    }

    public bool IsLocked() {
        return locked;
    }
}
