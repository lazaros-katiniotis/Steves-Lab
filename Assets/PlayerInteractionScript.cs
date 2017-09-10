using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionScript : MonoBehaviour {

    private GameObject obj;

    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("Player Interaction Collider entered " + collision.gameObject.transform.name + "'s " + collision.name);
        obj = collision.gameObject;
    }

    public GameObject GetLastObjectInteracted() {
        return obj;
    }
}
