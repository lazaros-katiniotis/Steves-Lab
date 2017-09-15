using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviousRoomColliderScript : MonoBehaviour {

    private DoorScript doorScript;

    private void Start() {
        doorScript = GetComponentInParent<DoorScript>();
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            collision.gameObject.GetComponent<PlayerController>().SetCurrentRoom(doorScript.GetPreviousRoom());
        }
    }
}
