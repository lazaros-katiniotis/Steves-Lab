using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorIndicatorInteractionScript : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision) {
        GameObject obj = collision.transform.parent.gameObject;
        string tag = obj.tag;
        //Debug.Log(this.transform.parent.name + "'s OnTriggerEnter2D() called:" + obj.name + " entered it's AoI");
        switch (tag) {
            case "Player":
            Debug.Log("Player is Inside Door Indicator AoI!");
            PlayerController player = obj.GetComponent<PlayerController>();
            player.SetLastInteractedObject(this.transform.parent.gameObject);
            break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        GameObject obj = collision.transform.parent.gameObject;
        string tag = obj.tag;
        Debug.Log(this.transform.parent.name + "'s OnTriggerExit2D() called:" + obj.name + " exited it's AoI");
        switch (tag) {
            case "Player":
            Debug.Log("exited Door Indicator AoI!");
            PlayerController player = obj.GetComponent<PlayerController>();
            player.SetLastInteractedObject(null);
            break;
        }
        obj = null;
    }
}
