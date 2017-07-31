using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : TogglableObject {

    public MachineScript terminal;
    public Transform closedSpriteTransform;
    public Transform openSpriteTransform;
    public Transform lockedLightTransform;
    public Transform unlockedLightTransform;

    public RoomScript previousRoom;
    public RoomScript nextRoom;


    private bool locked = true;
    private bool enteredDoorArea = false;

    // Use this for initialization
    void Start() {
        Lock();
        if (activeOnStart) {
            Unlock();
            TurnOff();
        }
    }

    public RoomScript GetNextRoom() {
        return nextRoom;
    }


    public RoomScript GetPreviousRoom() {
        return previousRoom;
    }

    public MachineScript GetTerminal() {
        return terminal;
    }

    void Update() {

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        //Debug.Log("OnTriggerEnter!" + this.transform.name);
        if (enteredDoorArea) {
            return;
        }
        if (!IsLocked() && !enteredDoorArea) {
            if (collision.CompareTag("Player")) {
                TurnOn();
            }
        }
        enteredDoorArea = true;
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (!IsLocked()) {
            if (collision.CompareTag("Player")) {
                TurnOn();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        //Debug.Log("OnTriggerExit!" + this.transform.name);
        if (!IsLocked() && !collision.gameObject.GetComponent<Collider2D>().IsTouching(this.GetComponent<Collider2D>())) {
            if (collision.CompareTag("Player")) {
                TurnOff();
            }
            enteredDoorArea = false;
        }
    }

    public void Lock() {
        TurnOff();
        locked = true;
        lockedLightTransform.gameObject.SetActive(true);
        unlockedLightTransform.gameObject.SetActive(false);
    }

    public void Unlock() {
        TurnOn();
        locked = false;
        lockedLightTransform.gameObject.SetActive(false);
        unlockedLightTransform.gameObject.SetActive(true);
    }

    public bool IsLocked() {
        return locked;
    }

    public override void Toggle() {

    }

    public override void TurnOn() {
        closedSpriteTransform.gameObject.SetActive(false);
        openSpriteTransform.gameObject.SetActive(true);
    }

    public override void TurnOff() {
        closedSpriteTransform.gameObject.SetActive(true);
        openSpriteTransform.gameObject.SetActive(false);
    }
}
