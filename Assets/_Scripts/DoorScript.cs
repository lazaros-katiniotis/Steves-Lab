using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {

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
        CloseDoor();
        locked = true;
    }

    public RoomScript GetNextRoom() {
        return nextRoom;
    }


    public RoomScript GetPreviousRoom() {
        return previousRoom;
    }

    void Update() {

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("OnTriggerEnter!" + this.transform.name);
        if (enteredDoorArea) {
            return;
        }
        if (!IsLocked() && !enteredDoorArea) {
            if (collision.CompareTag("Player")) {
                OpenDoor();
            }
        }
        enteredDoorArea = true;
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (!IsLocked()) {
            if (collision.CompareTag("Player")) {
                OpenDoor();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        Debug.Log("OnTriggerExit!" + this.transform.name);
        if (!IsLocked() && !collision.gameObject.GetComponent<Collider2D>().IsTouching(this.GetComponent<Collider2D>())) {
            if (collision.CompareTag("Player")) {
                CloseDoor();
            }
            enteredDoorArea = false;
        }
    }

    public void Lock() {
        CloseDoor();
        locked = true;
        lockedLightTransform.gameObject.SetActive(true);
        unlockedLightTransform.gameObject.SetActive(false);
    }

    public void Unlock() {
        OpenDoor();
        locked = false;
        lockedLightTransform.gameObject.SetActive(false);
        unlockedLightTransform.gameObject.SetActive(true);
    }

    public void OpenDoor() {
        closedSpriteTransform.gameObject.SetActive(false);
        openSpriteTransform.gameObject.SetActive(true);
    }

    public void CloseDoor() {
        closedSpriteTransform.gameObject.SetActive(true);
        openSpriteTransform.gameObject.SetActive(false);
    }

    public bool IsLocked() {
        return locked;
    }
}
