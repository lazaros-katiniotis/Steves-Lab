using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractionScript : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log(collision.transform.parent.name + " entered door's interaction trigger collider.");
    }
}
