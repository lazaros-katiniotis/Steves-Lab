using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameScript : MonoBehaviour {

    public int damage;
    private bool insideFlames;
    private static PlayerController player;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (player == null) {
            player = GameManager.GetInstance().player;
        }
        if (insideFlames) {
            if (!player.WasRecentlyHurt()) {
                player.Hurt(damage);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            insideFlames = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            insideFlames = false;
        }
    }

}
