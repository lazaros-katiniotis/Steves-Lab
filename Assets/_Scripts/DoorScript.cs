using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {

    public Transform closedSpriteTransform;
    public Transform openSpriteTransform;
    public Transform lockedLightTransform;
    public Transform unlockedLightTransform;

    private bool locked = true;

    // Use this for initialization
    void Start() {
        CloseDoor();
        locked = true;
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!IsLocked()) {
            if (collision.CompareTag("Player")) {
                OpenDoor();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (!IsLocked()) {
            if (collision.CompareTag("Player")) {
                OpenDoor();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (!IsLocked()) {
            if (collision.CompareTag("Player")) {
                CloseDoor();
            }
        }
    }

    public void Lock() {
        CloseDoor();
        locked = true;
        lockedLightTransform.gameObject.SetActive(true);
        unlockedLightTransform.gameObject.SetActive(false);
    }

    public void Unlock() {
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
