﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D rb;
    public float speed = 5f;

    // Use this for initialization
    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private Vector2 movementDirection;

    // Update is called once per frame
    void Update() {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        movementDirection.Set(h, v);
        //movementDirection = movementDirection.normalized;
    }

    void FixedUpdate() {
        //Debug.Log("force: " + movementDirection * speed);
        //rb.AddForce(movementDirection * speed);
        rb.MovePosition(rb.position + movementDirection * speed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("COLLISION ENTER!");
    }


}
