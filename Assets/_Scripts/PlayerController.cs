﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : Actor {

    private Rigidbody2D rb;
    public GameObject inventory;

    private InventoryManager inventoryManager;
    private Vector2 velocity;

    public float playerMaxHitPoints;
    public float boxGrabRange;
    public float playerHitPoints;
    public float playerOxygenLevel;
    private bool oxygenDepleting;

    public Transform pickupTransform;
    private CircleCollider2D circleCollider;
    private BoxCollider2D pickupCollider;

    //public TextMeshProUGUI roomOxygenText;
    //public TextMeshProUGUI playerOxygenText;
    //public TextMeshProUGUI playerHpText;

    private Animator animator;

    private Vector2 movementDirection;
    private Vector2 prevMovementDirection;

    public List<Texture> animationNormals;
    private Material material;

    public HealthBarScript healthBar;
    public HealthBarScript oxygenBar;

    private GameObject lastInteractableObjectInRange;
    //private TogglableObject lastInteractedObject;

    private float animationWaitTimer;
    private float animationWaitDuration;

    public AnimationCurve hurtCurve;
    private bool dead = false;
    private bool hurt;
    private bool dazed;
    private Vector2 knockbackForce;
    private Vector2 currentKnockbackForce;
    private BoxScript objectDragged;
    private Vector2 relativePosition;

    private Stack<EnergyObject> interactedEnergyObjects;

    public enum AnimationState {
        RUN_UP, RUN_DOWN, RUN_LEFT, RUN_RIGHT, IDLE_UP, IDLE_DOWN, IDLE_LEFT, IDLE_RIGHT, DEATH, START, CONTINUE
    }

    private AnimationState currentAnimationState;
    private AnimationState prevAnimationState;

    private void Awake() {
        //interactionScript = GetComponentInChildren<PlayerInteractionScript>();
        pickupCollider = pickupTransform.GetComponent<BoxCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        inventoryManager = inventory.GetComponent<InventoryManager>();
        animator = GetComponentInChildren<Animator>();
        material = GetComponentInChildren<Renderer>().material;
        interactedEnergyObjects = new Stack<EnergyObject>();
        bool value = DataManager.GetInstance().HasGameJustStarted();
        value = false;
        if (value) {
            prevAnimationState = AnimationState.START;
            currentAnimationState = AnimationState.START;
            animator.Play("PlayerStart");
            animationWaitDuration = 4.5f;
            DataManager.GetInstance().SetGameJustStarted(false);
        } else {
            animator.Play("PlayerContinue");
            prevAnimationState = AnimationState.CONTINUE;
            currentAnimationState = AnimationState.CONTINUE;
            animationWaitDuration = 0.6f;
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

    //public void Hurt(int damage) {
    //    hurt = true;
    //    playerHitPoints -= damage;
    //}

    public IEnumerator Hurt(int damage, float duration) {
        float elapsed = 0.0f;
        Color alphaColor = Color.white;
        float blinkSpeedConstant = 12;
        float blinkSpeed = blinkSpeedConstant;
        if (hurt == false) {
            hurt = true;
            playerHitPoints -= damage;
            while (elapsed < duration) {
                elapsed += Time.deltaTime;
                float scaledElapsed = elapsed / duration;
                float cutoff = hurtCurve.Evaluate(scaledElapsed);
                if (scaledElapsed >= 0.66f) {
                    blinkSpeed = 1.5f * blinkSpeedConstant;
                }
                float alpha = (Mathf.Sign(Mathf.Sin(blinkSpeed * Mathf.PI * scaledElapsed)) + 1) / 2.0f;
                material.SetFloat("_HurtCutoff", cutoff);
                alphaColor.a = alpha;
                material.SetColor("_Color", alphaColor);
                yield return null;
            }
            hurt = false;
        }
        yield return null;
    }

    public void Knockback(Vector2 force) {
        knockbackForce = force;
        currentKnockbackForce = force;
        StartCoroutine(Daze(0.25f));
    }

    private IEnumerator Daze(float duration) {
        dazed = true;
        float elapsed = 0.0f;
        while (elapsed < duration) {
            elapsed += Time.deltaTime;
            yield return null;
        }
        dazed = false;
        yield return null;
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

    // Update is called once per frame
    void Update() {
        healthBar.SetValue("_Cutoff", playerHitPoints / playerMaxHitPoints);

        oxygenBar.SetCutoffValue(playerOxygenLevel);
        oxygenBar.SetHighlightValue(playerOxygenLevel);

        animationWaitTimer += Time.deltaTime;
        if (animationWaitTimer < animationWaitDuration) {
            return;
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

        //if (dragging) {
        //    Vector3 delta = objectDragged.transform.position - this.transform.position;
        //    float distance = Mathf.Sqrt(delta.x * delta.x + delta.y * delta.y);
        //    if (boxGrabRange < distance) {
        //        objectDragged.Toggle(this);
        //    }
        //}

        UpdateDraggedObject();

        UpdatePlayerOxygenLevel();
        UpdatePlayerHitPoints();

        KeyboardInput();

        velocity = movementDirection * speed * Time.fixedDeltaTime;

        UpdatePlayerGUI();
    }

    private float angularSpeed = 90;
    private float targetRotationAngle = 0;
    private float currentRotationAngle = 0;
    private float rotationDirection = 0;

    private bool keepUpdating = false;
    private bool shouldStop = false;

    private void UpdateDraggedObject() {
        if (objectDragged) {
            if (keepUpdating) {
                float deltaAngle = angularSpeed * Time.deltaTime * 5.0f;
                if (rotationDirection == -1) {
                    if (currentRotationAngle + deltaAngle >= targetRotationAngle) {
                        if (!shouldStop) {
                            deltaAngle = targetRotationAngle - currentRotationAngle;
                            shouldStop = true;
                        }
                    }
                } else {
                    if (currentRotationAngle + deltaAngle <= targetRotationAngle) {
                        if (!shouldStop) {
                            deltaAngle = targetRotationAngle - currentRotationAngle;
                            shouldStop = true;
                        }
                    }
                }
                currentRotationAngle += deltaAngle;

                objectDragged.transform.RotateAround(circleCollider.bounds.center, Vector3.forward, deltaAngle);
                objectDragged.GetInteractionScript().transform.Rotate(0, 0, -deltaAngle);

                if (shouldStop) {
                    keepUpdating = false;
                }
            }
        }
    }

    private IEnumerator LerpToDraggedObject() {
        float elapsed = 0.0f;
        if (objectDragged) {
            Vector3 boxPos = objectDragged.transform.position;
            while (elapsed < 1.0f) {
                elapsed += Time.deltaTime * 10.0f;
                this.transform.position = Vector3.Lerp(this.transform.position, boxPos + objectDragged.bottom, elapsed);
                yield return null;
            }
            objectDragged.transform.SetParent(this.transform);
        }
        yield return null;
    }

    private float h;
    private float v;
    private float previousNormalIndex = 0;

    private void KeyboardInput() {
        h = 0;
        v = 0;
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            playerHitPoints = 0;
            CheckIfDead();
        }
        if (Input.GetButtonDown("Use")) {
            TogglableObject obj = null;
            if (lastInteractableObjectInRange != null) {
                obj = lastInteractableObjectInRange.GetComponentInParent<TogglableObject>();
            }
            if (dragging) {
                GetComponentInChildren<BoxScript>().Toggle(this);
            }
            if (obj == null) {
                return;
            }
            //SetLastInteractedObject(obj.gameObject);
            AddToInteractedObjectsStack(obj);
            obj.Toggle(this);
        }

        if (!dragging) {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");

        } else {
            if (!keepUpdating) {
                if (Input.GetButtonDown("Horizontal")) {
                    //lastInteractedObject.transform.RotateAround(this.circleCollider.bounds.center, Vector3.forward, -90 * Input.GetAxisRaw("Horizontal"));
                    rotationDirection = Input.GetAxisRaw("Horizontal");
                    angularSpeed = -rotationDirection * 90.0f;
                    targetRotationAngle += angularSpeed;
                    shouldStop = false;
                    keepUpdating = true;
                }
            }
            if (relativePosition.y == -1.0f) {
                v = Input.GetAxisRaw("Vertical") * objectDragged.transform.up.y;
                h = Input.GetAxisRaw("Vertical") * objectDragged.transform.up.x;
            } else if (relativePosition.y == 1.0f) {
                v = -Input.GetAxisRaw("Vertical") * objectDragged.transform.up.y;
                h = -Input.GetAxisRaw("Vertical") * objectDragged.transform.up.x;
            } else if (relativePosition.x == -1.0f) {
                v = Input.GetAxisRaw("Vertical") * objectDragged.transform.right.y;
                h = Input.GetAxisRaw("Vertical") * objectDragged.transform.right.x;
            } else if (relativePosition.x == 1.0f) {
                v = -Input.GetAxisRaw("Vertical") * objectDragged.transform.right.y;
                h = -Input.GetAxisRaw("Vertical") * objectDragged.transform.right.x;
            }
        }

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
        oxygenDepleting = false;
        if (currentRoom == null) {
            playerOxygenLevel -= Time.deltaTime * 0.0533f;
            oxygenDepleting = true;
        } else {
            if (currentRoom.GetCurrentOxygenPercent() <= 0.0f) {
                playerOxygenLevel -= Time.deltaTime * 0.0333f;
                oxygenDepleting = true;
            } else if (currentRoom.GetCurrentOxygenPercent() < 1.0f) {
                //playerOxygenLevel += Time.deltaTime * 0.075f;
                playerOxygenLevel += Time.deltaTime * 0.5333f;
            } else if (currentRoom.GetCurrentOxygenPercent() >= 1.0f) {
                //playerOxygenLevel += Time.deltaTime * 0.075f;
                playerOxygenLevel += Time.deltaTime * 0.533f;
            }
        }

        if (oxygenDepleting) {
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
        rb.AddForce(currentKnockbackForce);
        currentKnockbackForce *= 0.75f;
        //bool xAxis = currentKnockbackForce.x < (knockbackForce.x / 2);
        //bool yAxis = currentKnockbackForce.y < (knockbackForce.y / 2);
        if (dazed) {
            velocity = Vector2.zero;
        }
        rb.MovePosition(rb.position + velocity);

    }

    public void SetLastInteractableObjectInRange(GameObject obj) {
        lastInteractableObjectInRange = obj;
    }

    //public void SetLastInteractedObject(GameObject obj) {
    //    lastInteractedObject = obj;
    //}

    public bool IsOxygenDepleting() {
        return oxygenDepleting;
    }

    public Vector2 GetMovementDirection() {
        return movementDirection;
    }

    public BoxCollider2D GetPickupCollider() {
        return pickupCollider;
    }

    //hardcoded
    public override void UpdateDraggingState(bool isDragging, Vector2 relativePosition, BoxScript obj) {
        dragging = isDragging;
        //if (dragging == true) {
        //    this.circleCollider.enabled = false;
        //    this.boxCollider.enabled = true;
        //    this.boxCollider.offset = offset;
        //    this.boxCollider.size = size;
        //} else {
        //    this.boxCollider.enabled = false;
        //    this.circleCollider.enabled = true;
        //}
        this.relativePosition = relativePosition;
        objectDragged = obj;
        StartCoroutine(LerpToDraggedObject());
    }

    public void AddToInteractedObjectsStack(TogglableObject obj) {
        if (obj.GetType().Equals(typeof(BoxScript))) {
            return;
        }
        EnergyObject energyObj = obj as EnergyObject;
        if (!interactedEnergyObjects.Contains(energyObj)) {
            interactedEnergyObjects.Push(energyObj);
        }
    }

    public Stack<EnergyObject> GetInteractedEnergyObjects() {
        return interactedEnergyObjects;
    }
}
