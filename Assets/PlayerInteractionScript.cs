using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionScript : MonoBehaviour {

    /*
    private GameObject obj;
    private bool insideDoorArea;

    private void OnTriggerEnter2D(Collider2D collision) {
        obj = collision.transform.parent.gameObject;
        string tag = obj.tag;
        Debug.Log(this.transform.parent.name + "'s OnTriggerEnter2D() called:" + obj.name + " entered it's AoI");
        switch (tag) {
            case "Door":
            Debug.Log("Inside door AoI!");
            insideDoorArea = true;
            break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        obj = collision.transform.parent.gameObject;
        string tag = obj.tag;
        Debug.Log(this.transform.parent.name + "'s OnTriggerExit2D() called:" + obj.name + " exited it's AoI");
        switch (tag) {
            case "Door":
            Debug.Log("exited door AoI!");
            insideDoorArea = false;
            break;
        }
        obj = null;
    }

    public GameObject GetLastObjectInteracted() {
        return obj;
    }
    */
}
