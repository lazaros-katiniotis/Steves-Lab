using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObjectScript : MonoBehaviour {

    public enum PickupObjectType {
        KEYCARD
    };

    public PickupObjectType type;
    private Vector2 startPosition;
    private Vector2 nextPosition;

    private float elapsed;

    // Use this for initialization
    void Start() {
        startPosition = this.transform.position;
        nextPosition = startPosition + new Vector2(0, 0.15f);
    }

    // Update is called once per frame
    void Update() {
        BobObject();
    }

    private void BobObject() {
        elapsed += Time.deltaTime;
        float duration = 1.0f;
        this.transform.position = Vector2.Lerp(startPosition, nextPosition, elapsed / duration);
        if (elapsed > 1.0f * duration) {
            Vector2 tempPosition = startPosition;
            startPosition = nextPosition;
            nextPosition = tempPosition;
            elapsed = 0.0f;
        }
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
