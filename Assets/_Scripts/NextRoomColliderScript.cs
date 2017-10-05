using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextRoomColliderScript : MonoBehaviour {

    private DoorScript doorScript;

    private void Start() {
        doorScript = GetComponentInParent<DoorScript>();
    }

    private void OnTriggerStay2D(Collider2D collision) {
        //if (collision.CompareTag("Player")) {
            collision.gameObject.GetComponentInParent<PlayerController>().SetCurrentRoom(doorScript.GetNextRoom());
        //}
    }
}
