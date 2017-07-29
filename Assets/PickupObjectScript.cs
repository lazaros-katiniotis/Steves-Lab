using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObjectScript : MonoBehaviour {

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag.Equals("Player")) {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            //play pickup sound
            player.GetInventoryManager().AddItem(this.gameObject);
            StartCoroutine(RemoveFromScene());
        }
    }

    public IEnumerator RemoveFromScene() {
        Destroy(this.gameObject);
        yield return null;
    }

    private void OnCollisionEnter2D(Collision2D collision) {

    }
}
