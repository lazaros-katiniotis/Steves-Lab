using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractionScript : MonoBehaviour {

    private DoorScript doorScript;

    private void Awake() {
        doorScript = GetComponentInParent<DoorScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        GameObject obj = collision.transform.parent.gameObject;
        string tag = obj.tag;
        //Debug.Log(this.transform.parent.name + "'s OnTriggerEnter2D() called:" + obj.name + " entered it's AoI");
        switch (tag) {
            case "Player":
            //Debug.Log("Player is Inside Door AoI!");
            //doorScript.Open();
            doorScript.SetPlayerInRange(true);
            break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        GameObject obj = collision.transform.parent.gameObject;
        string tag = obj.tag;
        //Debug.Log(this.transform.parent.name + "'s OnTriggerExit2D() called:" + obj.name + " exited it's AoI");
        switch (tag) {
            case "Player":
            //Debug.Log("Player exited Inside Door AoI!");
            //doorScript.Close();
            doorScript.SetPlayerInRange(false);
            break;
        }
    }

}
