using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : Actor {

    private Rigidbody2D rb;
    public float speed = 5f;
    public GameObject inventory;

    private InventoryManager inventoryManager;
    private Vector2 velocity;

    public float playerMaxHitPoints;
    public float playerHitPoints;
    public float playerOxygenLevel;

    public RoomScript currentRoom;

    //public TextMeshProUGUI roomOxygenText;
    //public TextMeshProUGUI playerOxygenText;
    //public TextMeshProUGUI playerHpText;

    private Animator animator;

    private Vector2 movementDirection;
    private Vector2 prevMovementDirection;

    public List<Texture> animationNormals;
    private Material material;
    private bool dead = false;

    public HealthBarScript healthBar;
    public HealthBarScript oxygenBar;

    //private PlayerInteractionScript interactionScript;

    private GameObject lastInteractedObject;

    private float waitTimer;
    private float waitDuration;

    private bool hurt;
    private float hurtTimer;

    public enum AnimationState {
        RUN_UP, RUN_DOWN, RUN_LEFT, RUN_RIGHT, IDLE_UP, IDLE_DOWN, IDLE_LEFT, IDLE_RIGHT, DEATH, START, CONTINUE
    }

    private AnimationState currentAnimationState;
    private AnimationState prevAnimationState;

    private void Awake() {
        //interactionScript = GetComponentInChildren<PlayerInteractionScript>();
    }

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        inventoryManager = inventory.GetComponent<InventoryManager>();
        animator = GetComponentInChildren<Animator>();
        material = GetComponentInChildren<Renderer>().material;
        bool value = DataManager.GetInstance().HasGameJustStarted();
        value = false;
        if (value) {
            prevAnimationState = AnimationState.START;
            currentAnimationState = AnimationState.START;
            animator.Play("PlayerStart");
            waitDuration = 4.5f;
            DataManager.GetInstance().SetGameJustStarted(false);
        } else {
            animator.Play("PlayerContinue");
            prevAnimationState = AnimationState.CONTINUE;
            currentAnimationState = AnimationState.CONTINUE;
            waitDuration = 0.6f;
        }
    }

    public void DisableBars() {
        healthBar.gameObject.SetActive(false);
        oxygenBar.gameObject.SetActive(false);
    }

    public void ReplenishHealth(float value) {
        playerHitPoints += value;
    }

    public void ReplenishOxygen(float value) {
        playerOxygenLevel += value;

        if (playerOxygenLevel < 0.0f) {
            playerOxygenLevel = 0.0f;
        } else if (playerOxygenLevel > 1.0f) {
            playerOxygenLevel = 1.0f;
        }
    }

    public bool WasRecentlyHurt() {
        return hurt;
    }

    public void Hurt(int damage) {
        hurt = true;
        playerHitPoints -= damage;
    }

    public bool IsHealthFull() {
        return (playerHitPoints == playerMaxHitPoints);
    }

    public bool IsOxygenFull() {
        return (playerOxygenLevel == 1.0f);
    }

    public void PlayDeathAnimation() {
        movementDirection = Vector2.zero;
        rb.velocity = Vector2.zero;
        animator.Play("PlayerDeath");
        UpdateMaterialNormal(8);

    }

    public void SetCurrentRoom(RoomScript room) {
        currentRoom = room;
    }

    // Update is called once per frame
    void Update() {
        healthBar.SetValue("_Cutoff", playerHitPoints / playerMaxHitPoints);

        oxygenBar.SetCutoffValue(playerOxygenLevel);
        oxygenBar.SetHighlightValue(playerOxygenLevel);

        waitTimer += Time.deltaTime;
        if (waitTimer < waitDuration) {
            return;
        }

        if (WasRecentlyHurt()) {
            hurtTimer += Time.deltaTime;
            if (hurtTimer > 1.0f) {
                hurt = false;
                hurtTimer = 0.0f;
            }
        }


        CheckIfDead();
        if (dead) {
            return;
        }

        if (currentRoom != null) {
            //roomOxygenText.text = "Room Oxygen%: " + currentRoom.GetCurrentOxygenPercent().ToString("F2");
        } else {
            //roomOxygenText.text = "No Room";
        }

        UpdatePlayerOxygenLevel();
        UpdatePlayerHitPoints();

        KeyboardInput();

        velocity = movementDirection * speed * Time.fixedDeltaTime;

        UpdatePlayerGUI();
    }

    private float h;
    private float v;
    private float previousNormalIndex = 0;

    private void KeyboardInput() {
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            playerHitPoints = 0;
            CheckIfDead();
        }
        if (Input.GetButtonDown("Use")) {
            if (lastInteractedObject == null) {
                return;
            }
            TogglableObject obj = lastInteractedObject.GetComponentInParent<TogglableObject>();
            obj.Toggle(this);
        }

        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        movementDirection.Set(h, v);

        UpdateAnimation();

        prevAnimationState = currentAnimationState;
        prevMovementDirection = movementDirection;
    }

    private void UpdateAnimation() {
        //if (currentAnimationState == AnimationState.START) {

        //}

        if (h == 1 && v == 0) {
            animator.Play("PlayerRunRight");
            currentAnimationState = AnimationState.RUN_RIGHT;
            UpdateMaterialNormal(3);
        } else if (h == -1 && v == 0) {
            animator.Play("PlayerRunLeft");
            currentAnimationState = AnimationState.RUN_LEFT;
            UpdateMaterialNormal(1);
        } else if (v == 1) {
            animator.Play("PlayerRunUp");
            currentAnimationState = AnimationState.RUN_UP;
            UpdateMaterialNormal(0);
        } else if (v == -1) {
            animator.Play("PlayerRunDown");
            currentAnimationState = AnimationState.RUN_DOWN;
            UpdateMaterialNormal(2);
        }

        if (movementDirection.Equals(Vector2.zero)) {
            switch (prevAnimationState) {
                case AnimationState.RUN_UP:
                animator.Play("PlayerIdleUp");
                UpdateMaterialNormal(4);
                break;
                case AnimationState.RUN_LEFT:
                animator.Play("PlayerIdleLeft");
                UpdateMaterialNormal(5);
                break;
                case AnimationState.RUN_DOWN:
                animator.Play("PlayerIdleDown");
                UpdateMaterialNormal(6);
                break;
                case AnimationState.RUN_RIGHT:
                animator.Play("PlayerIdleRight");
                UpdateMaterialNormal(7);
                break;
                default:
                break;
            }
        }
    }

    private void UpdateMaterialNormal(int index) {
        if (previousNormalIndex.Equals(index)) {
            return;
        }
        material.SetTexture("_BumpMap", animationNormals[index]);
        previousNormalIndex = index;
    }

    public void UpdatePlayerOxygenLevel() {
        bool depleting = false;
        if (currentRoom == null) {
            playerOxygenLevel -= Time.deltaTime * 0.333f;
            depleting = true;
        } else {
            if (currentRoom.GetCurrentOxygenPercent() <= 0.0f) {
                playerOxygenLevel -= Time.deltaTime * 0.0333f;
                depleting = true;
            } else if (currentRoom.GetCurrentOxygenPercent() < 1.0f) {
                playerOxygenLevel += Time.deltaTime * 0.02f;
            } else if (currentRoom.GetCurrentOxygenPercent() >= 1.0f) {
                playerOxygenLevel += Time.deltaTime * 0.05f;
            }
        }

        if (depleting) {
            oxygenBar.StartFlash();
        } else {
            oxygenBar.StopFlash();
        }

        if (playerOxygenLevel < 0.0f) {
            playerOxygenLevel = 0.0f;
        } else if (playerOxygenLevel > 1.0f) {
            playerOxygenLevel = 1.0f;
        }
    }

    public void UpdatePlayerGUI() {
        //playerOxygenText.text = "Player Oxygen%: " + playerOxygenLevel.ToString("F2");
        //playerHpText.text = "Player HP: " + playerHitPoints;
    }

    private float oxygenDamageElapsed;

    public void UpdatePlayerHitPoints() {
        if (playerOxygenLevel <= 0.0f) {
            oxygenDamageElapsed += Time.deltaTime;
            if (oxygenDamageElapsed > 1.0f) {
                playerHitPoints -= 5;
                oxygenDamageElapsed -= 1.0f;
            }
        } else {
            oxygenDamageElapsed = 0.0f;
        }

        if (playerHitPoints > playerMaxHitPoints) {
            playerHitPoints = playerMaxHitPoints;
        }
    }

    public void CheckIfDead() {
        if (playerHitPoints <= 0.0f) {
            playerHitPoints = 0.0f;
            dead = true;
            PlayDeathAnimation();
            GameManager.GetInstance().GameOver();
        }
    }

    public bool IsDead() {
        return (playerHitPoints == 0);
    }

    public override InventoryManager GetInventory() {
        return inventoryManager;
    }

    void FixedUpdate() {
        if (dead) {
            return;
        }
        //Debug.Log("force: " + movementDirection * speed);
        //rb.AddForce(movementDirection * speed);

        rb.MovePosition(rb.position + velocity);

    }

    public void SetLastInteractedObject(GameObject obj) {
        lastInteractedObject = obj;
    }
}
