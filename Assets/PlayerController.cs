using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D rb;
    public float speed = 5f;
    public GameObject inventory;

    private InventoryManager inventoryManager;

    private Vector2 velocity;

    private float oxygenLevel;

    // Use this for initialization
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        inventoryManager = inventory.GetComponent<InventoryManager>();
    }

    private Vector2 movementDirection;

    // Update is called once per frame
    void Update() {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        movementDirection.Set(h, v);

        velocity = movementDirection * speed * Time.fixedDeltaTime;

        if (Input.GetButtonDown("Use")) {
            foreach (GameObject obj in MachineManager.GetInstance().machines) {
                float xDelta = Mathf.Abs(obj.transform.position.x - this.transform.position.x);
                float yDelta = Mathf.Abs(obj.transform.position.y - this.transform.position.y);
                float distance = Mathf.Sqrt(xDelta * xDelta + yDelta * yDelta);
                if (distance < 1.0f) {
                    obj.GetComponent<MachineScript>().ToggleMachineFunction();
                }
            }
        }
        //movementDirection = movementDirection.normalized;
    }

    public InventoryManager GetInventoryManager() {
        return inventoryManager;
    }

    void FixedUpdate() {
        //Debug.Log("force: " + movementDirection * speed);
        //rb.AddForce(movementDirection * speed);

        rb.MovePosition(rb.position + velocity);

    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("COLLISION ENTER!");
    }


}
