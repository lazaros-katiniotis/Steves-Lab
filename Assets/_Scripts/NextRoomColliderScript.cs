using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextRoomColliderScript : MonoBehaviour {

    private DoorScript door;

    private void Start() {
        door = this.transform.parent.GetComponent<DoorScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            collision.gameObject.GetComponent<PlayerController>().SetCurrentRoom(door.GetNextRoom());
        }
    }
}
