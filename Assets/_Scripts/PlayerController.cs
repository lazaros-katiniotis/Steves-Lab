using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D rb;
    public float speed = 5f;
    public GameObject inventory;

    private InventoryManager inventoryManager;

    private Vector2 velocity;

    private float oxygenLevel;

    public float playerOxygenLevel;
    public float playerHitPoints;

    public RoomScript currentRoom;
    public TextMeshProUGUI roomOxygenText;
    public TextMeshProUGUI playerOxygenText;
    public TextMeshProUGUI playerHpText;

    // Use this for initialization
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        inventoryManager = inventory.GetComponent<InventoryManager>();
        //playerHitPoints = 100;
        playerOxygenLevel = 1.0f;
        StartCoroutine(CheckIfDead());
    }

    public IEnumerator CheckIfDead() {
        while (playerHitPoints > 0.0f) {
            yield return null;
        }
        Destroy(this.gameObject);
        GameManager.GetInstance().GameOver(GetComponentInChildren<Camera>());
        Debug.Log("PLAYER IS DEAD!!");
    }

    public void SetCurrentRoom(RoomScript room) {
        currentRoom = room;
    }

    private Vector2 movementDirection;

    // Update is called once per frame
    void Update() {
        if (currentRoom != null) {
            roomOxygenText.text = "Room Oxygen%: " + currentRoom.GetCurrentOxygenPercent().ToString("F2");
        } else {
            roomOxygenText.text = "No Room";
        }

        UpdatePlayerOxygenLevel();
        UpdatePlayerHitPoints();

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        movementDirection.Set(h, v);

        velocity = movementDirection * speed * Time.fixedDeltaTime;

        if (Input.GetButtonDown("Use")) {
            foreach (GameObject obj in GameManager.GetInstance().machines) {
                float xDelta = Mathf.Abs(obj.transform.position.x - this.transform.position.x);
                float yDelta = Mathf.Abs(obj.transform.position.y - this.transform.position.y);
                float distance = Mathf.Sqrt(xDelta * xDelta + yDelta * yDelta);
                if (distance < 1.25f) {
                    obj.GetComponent<MachineScript>().ToggleMachineFunction();
                }
            }
        }

        UpdatePlayerGUI();
        //movementDirection = movementDirection.normalized;
    }

    public void UpdatePlayerOxygenLevel() {
        if (currentRoom == null) {
            playerOxygenLevel -= Time.deltaTime * 0.333f;
        } else {
            if (currentRoom.GetCurrentOxygenPercent() <= 0.0f) {
                playerOxygenLevel -= Time.deltaTime * 0.0333f;
            } else if (currentRoom.GetCurrentOxygenPercent() < 1.0f) {
                playerOxygenLevel += Time.deltaTime * 0.0333f;
            } else if (currentRoom.GetCurrentOxygenPercent() >= 1.0f) {
                playerOxygenLevel += Time.deltaTime * 0.5f;
            }
        }

        if (playerOxygenLevel < 0.0f) {
            playerOxygenLevel = 0.0f;
        } else if (playerOxygenLevel > 1.0f) {
            playerOxygenLevel = 1.0f;
        }
    }

    public void UpdatePlayerGUI() {
        playerOxygenText.text = "Player Oxygen%: " + playerOxygenLevel.ToString("F2");
        playerHpText.text = "Player HP: " + playerHitPoints;
    }

    private float oxygenDamageElapsed;

    public void UpdatePlayerHitPoints() {
        if (playerOxygenLevel <= 0.0f) {
            oxygenDamageElapsed += Time.deltaTime;
            if (oxygenDamageElapsed > 1.0f) {
                playerHitPoints -= 5;
                oxygenDamageElapsed -= 1.0f;
            }
        }
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

    }


}
