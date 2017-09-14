using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextRoomColliderScript : MonoBehaviour {

    private DoorScript doorScript;

    private void Start() {
        doorScript = GetComponentInParent<DoorScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            collision.gameObject.GetComponent<PlayerController>().SetCurrentRoom(doorScript.GetNextRoom());
        }
    }
}
