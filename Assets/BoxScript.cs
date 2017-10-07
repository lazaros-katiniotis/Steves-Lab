using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour {

    private Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        rb.MovePosition(rb.position);
    }
}
