using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameScript : MonoBehaviour {

    private bool insideFlames;
    private static PlayerController player;
    private Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
        animator.Play("FlameAnimation", 0, Random.Range(0.0f, 1.0f));
    }

    void Update() {
        if (player == null) {
            player = GameManager.GetInstance().player;
        }
        if (insideFlames) {
            if (!player.WasRecentlyHurt()) {
                //player.Hurt(10, 1.0f);
                StartCoroutine(player.Hurt(10, 1.0f));
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
