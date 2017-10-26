using System;
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

    private GameObject lastInteractedObject;

    private float animationWaitTimer;
    private float animationWaitDuration;

    public AnimationCurve hurtCurve;
    private bool dead = false;
    private bool hurt;
    private bool dazed;
    private Vector2 knockbackForce;
    private Vector2 currentKnockbackForce;
    private TogglableObject objectDragged;

    private Stack<EnergyObject> interactedEnergyObjects;

    public enum AnimationState {
        RUN_UP, RUN_DOWN, RUN_LEFT, RUN_RIGHT, IDLE_UP, IDLE_DOWN, IDLE_LEFT, IDLE_RIGHT, DEATH, START, CONTINUE
    }

    private AnimationState currentAnimationState;
    private AnimationState prevAnimationState;

    private void Awake() {
        //interactionScript = GetComponentInChildren<PlayerInteractionScript>();
        pickupCollider = pickupTransform.GetComponent<BoxCollider2D>();
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

        if (dragging) {
            Vector3 delta = objectDragged.transform.position - this.transform.position;
            float distance = Mathf.Sqrt(delta.x * delta.x + delta.y * delta.y);
            if (boxGrabRange < distance) {
                objectDragged.Toggle(this);
            }
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
            TogglableObject obj = null;
            if (lastInteractedObject == null) {
                BoxScript box;
                if (dragging) {
                    obj = GetComponentInChildren<BoxScript>();
                } else {
                    float distance = GameManager.GetInstance().GetNearestBox(out box);
                    Debug.Log("Distance: " + distance);
                    if (boxGrabRange > distance) {
                        obj = box;
                    }
                }
            } else {
                obj = lastInteractedObject.GetComponentInParent<TogglableObject>();
            }
            if (obj == null) {
                return;
            }
            AddToInteractedObjectsStack(obj);
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
        bool xAxis = currentKnockbackForce.x < (knockbackForce.x / 2);
        bool yAxis = currentKnockbackForce.y < (knockbackForce.y / 2);
        if (dazed) {
            velocity = Vector2.zero;
        }
        rb.MovePosition(rb.position + velocity);

    }

    public void SetLastInteractedObject(GameObject obj) {
        lastInteractedObject = obj;
    }

    public bool IsOxygenDepleting() {
        return oxygenDepleting;
    }

    public Vector2 GetMovementDirection() {
        return movementDirection;
    }

    public BoxCollider2D GetPickupCollider() {
        return pickupCollider;
    }

    public override void UpdateDraggingState(bool isDragging, TogglableObject obj) {
        dragging = isDragging;
        objectDragged = obj;
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
