using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFloor : MonoBehaviour {

    public Transform offSprite;
    public Transform onSprite;

    private bool active;

    private int entitiesCounter = 0;
    private bool occupied;

    public void Start() {
        active = false;
        offSprite.gameObject.SetActive(true);
        onSprite.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        entitiesCounter++;
        Debug.Log(collision.transform.gameObject);
        occupied = true;
        active = true;
        offSprite.gameObject.SetActive(false);
        onSprite.gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        entitiesCounter--;
    }

    private void OnTriggerStay2D(Collider2D collision) {

    }

    private void Update() {
        if (entitiesCounter == 0) {
            active = false;
            occupied = false;
            offSprite.gameObject.SetActive(true);
            onSprite.gameObject.SetActive(false);
        }
    }

    public bool IsActive() {
        return active;
    }
}
