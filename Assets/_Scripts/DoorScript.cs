using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : TogglableObject {

    public TerminalScript terminal;
    public Transform closedSpriteTransform;
    public Transform openSpriteTransform;
    public Transform lockedLightTransform;
    public Transform unlockedLightTransform;

    public RoomScript previousRoom;
    public RoomScript nextRoom;

    private bool lockWhenPlayerLeaves = false;
    private bool shouldLockDoor = false;

    private bool locked = true;
    private bool enteredDoorArea = false;
    private bool isInsideDoorArea = false;

    // Use this for initialization
    void Start() {
        //Lock();
        //if (activeOnStart) {
        //    Unlock();
        //    TurnOff();
        //}
    }

    public RoomScript GetNextRoom() {
        return nextRoom;
    }


    public RoomScript GetPreviousRoom() {
        return previousRoom;
    }

    public TerminalScript GetTerminalScript() {
        return terminal;
    }

    void Update() {
        //if (shouldLockDoor) {
        //    if (!isInsideDoorArea) {
        //        Debug.Log("Should lock door, so since player is not inside door area im locking the door.");
        //        Lock();
        //        shouldLockDoor = false;
        //    }
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        ////Debug.Log("OnTriggerEnter2D()");
        //if (enteredDoorArea) {
        //    return;
        //}
        //if (!IsLocked() && !enteredDoorArea) {
        //    if (collision.CompareTag("Player")) {
        //        TurnOn();
        //    }
        //}
        //enteredDoorArea = true;
    }

    private void OnTriggerStay2D(Collider2D collision) {
        ////Debug.Log("OnTriggerStay2D()");
        //if (!IsLocked()) {
        //    if (collision.CompareTag("Player")) {
        //        TurnOn();
        //    }
        //}
        //isInsideDoorArea = true;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        ////Debug.Log("OnTriggerExit2D()");
        //if (!IsLocked() && !collision.gameObject.GetComponent<Collider2D>().IsTouching(this.GetComponent<Collider2D>())) {
        //    if (collision.CompareTag("Player")) {
        //        if (!lockWhenPlayerLeaves) {
        //            TurnOff();
        //        }
        //    }
        //}
        //isInsideDoorArea = false;
        //enteredDoorArea = false;
    }

    public void Lock() {
        //Debug.Log("LOCK CALLED");
        TurnOff();
        locked = true;
        lockWhenPlayerLeaves = false;
        if (lockedLightTransform != null) {
            lockedLightTransform.gameObject.SetActive(true);
        }
        if (unlockedLightTransform != null) {
            unlockedLightTransform.gameObject.SetActive(false);
        }
    }

    public void Unlock() {
        //Debug.Log("UNLOCK CALLED");
        TurnOn();
        locked = false;
        if (lockedLightTransform != null) {
            lockedLightTransform.gameObject.SetActive(false);
        }
        if (unlockedLightTransform != null) {
            unlockedLightTransform.gameObject.SetActive(true);
        }
    }

    public override void Toggle() {
        Debug.Log("Doorscripts Toggle called");

        if (IsLocked()) {
            Debug.Log("Door is locked, lockwhenplayerLeaves = true, unlocking door");
            lockWhenPlayerLeaves = true;
            Unlock();
        } else {
            Debug.Log("Door not locked, should lock door");
            shouldLockDoor = true;
        }
    }


    public bool IsLocked() {
        return locked;
    }

    public override void TurnOn() {
        closedSpriteTransform.gameObject.SetActive(false);
        openSpriteTransform.gameObject.SetActive(true);
    }

    public override void TurnOff() {
        closedSpriteTransform.gameObject.SetActive(true);
        openSpriteTransform.gameObject.SetActive(false);
    }

    public void LockWhenPlayerLeaves() {
        lockWhenPlayerLeaves = true;
    }
}
