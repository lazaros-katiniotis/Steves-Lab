using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractionScript : MonoBehaviour {

    private NewDoorScript doorScript;

    private void Awake() {
        doorScript = GetComponentInParent<NewDoorScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        GameObject obj = collision.transform.parent.gameObject;
        string tag = obj.tag;
        //Debug.Log(this.transform.parent.name + "'s OnTriggerEnter2D() called:" + obj.name + " entered it's AoI");
        switch (tag) {
            case "Player":
            Debug.Log("Player is Inside Door AoI!");
            doorScript.TurnOn();
            break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        GameObject obj = collision.transform.parent.gameObject;
        string tag = obj.tag;
        //Debug.Log(this.transform.parent.name + "'s OnTriggerExit2D() called:" + obj.name + " exited it's AoI");
        switch (tag) {
            case "Player":
            Debug.Log("Player exited Inside Door AoI!");
            doorScript.TurnOff();
            break;
        }
    }

}
