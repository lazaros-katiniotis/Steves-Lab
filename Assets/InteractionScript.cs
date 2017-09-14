using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionScript : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision) {
        GameObject obj = collision.transform.parent.gameObject;
        string tag = obj.tag;
        switch (tag) {
            case "Player":
            Debug.Log("Player entered " + this.transform.parent.gameObject + "'s AoI!");
            PlayerController player = obj.GetComponent<PlayerController>();
            player.SetLastInteractedObject(this.transform.parent.gameObject);
            break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        GameObject obj = collision.transform.parent.gameObject;
        string tag = obj.tag;
        switch (tag) {
            case "Player":
            Debug.Log("Player exited " + this.transform.parent.gameObject + "'s AoI!");
            PlayerController player = obj.GetComponent<PlayerController>();
            player.SetLastInteractedObject(null);
            break;
        }
        obj = null;
    }
}
