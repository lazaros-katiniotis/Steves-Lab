using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour {
    [Range(-360, 360)]
    public float angle;

    public GameObject target;
    public LayerMask layerMask;
    public Light laserLight;
    public ParticleSystem sparkParticleSystem;

    private LineRenderer lineRenderer;

    public float defaultWidth;
    public float nextWidth;
    private float currentWidth;

    private Vector3 laserPointPosition;
    private Vector3 sparkPosition;

    private float intensityElapsed = 0;
    private float widthElapsed = 0;

    private PlayerController player;
    private Vector3 perpendicular;

    private LaserEmitterScript.LaserEmitterState state;

    private bool stateHandled;
    private bool emitting;

    private LaserEmitterScript laserEmitter;

    private float currentAngle;
    private float targetAngle;

    private void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
        laserEmitter = GetComponentInParent<LaserEmitterScript>();
        currentWidth = defaultWidth;
        laserPointPosition = new Vector3(0, 0, -0.05f);
        sparkPosition = new Vector3(0, 0, -0.15f);
        stateHandled = true;
    }

    public void Activate() {
        flicker = true;
        emitting = true;
    }

    public void Deactivate() {
        CastLaser();
        lineRenderer.widthMultiplier = 0.0f;
        SetParticleEmission(false);
        laserLight.enabled = false;
        currentWidth = 0.0f;
        emitting = false;
    }

    private bool flicker;

    void Update() {
        if (flicker) {
            CastLaser();
            LaserWidthAnimation();
            LaserLightAnimation();
        }
    }

    private IEnumerator HandleState(LaserEmitterScript.LaserEmitterStateEnum state, LaserEmitterScript.LaserEmitterData data) {
        if (!stateHandled) {
            stateHandled = true;
            Debug.Log("CURRENT STATE: " + state);
            switch (state) {
                case LaserEmitterScript.LaserEmitterStateEnum.TURN_ON:
                StartCoroutine(ToggleLaserBeam(nextWidth, true));
                break;
                case LaserEmitterScript.LaserEmitterStateEnum.TURN_OFF:
                StartCoroutine(ToggleLaserBeam(0.0f, false));
                break;
                case LaserEmitterScript.LaserEmitterStateEnum.WAIT:
                break;
                case LaserEmitterScript.LaserEmitterStateEnum.ROTATE:
                StartCoroutine(RotateLaserBeam(data.targetAngle, 10, data.duration));
                break;
                default:
                stateHandled = false;
                laserEmitter.PrepareNextState();
                break;
            }
        }
        yield return null;
    }

    private IEnumerator ToggleLaserBeam(float nextWidth, bool shouldEmit) {
        float elapsed = 0.0f;
        flicker = false;
        bool entered = false;
        while (elapsed <= 1.0f) {
            elapsed += Time.deltaTime * 5.0f;
            currentWidth = Mathf.Lerp(currentWidth, nextWidth, elapsed);
            if (elapsed > 0.15f) {
                if (!entered) {
                    entered = true;
                    SetParticleEmission(shouldEmit);
                    laserLight.enabled = shouldEmit;
                    emitting = shouldEmit;
                }
                if (nextWidth != 0.0f) {
                    CastLaser();
                }
            }
            lineRenderer.widthMultiplier = currentWidth;
            yield return null;
        }
        if (nextWidth != 0.0f) {
            flicker = true;
        }
        laserEmitter.PrepareNextState();
        yield return null;
    }

    private IEnumerator RotateLaserBeam(float next, float angularSpeed, float duration) {
        targetAngle += next / 2;
        if (Mathf.Abs(currentAngle) > 360) {
            currentAngle = currentAngle % 360;
            targetAngle = targetAngle % 360;
        }

        float minDelta = targetAngle - currentAngle;
        float direction = Mathf.Sign(minDelta);

        while (currentAngle != targetAngle) {
            float deltaAngle = direction * angularSpeed * Time.deltaTime;
            if (direction > 0) {
                if (currentAngle + deltaAngle > targetAngle) {
                    deltaAngle = targetAngle - currentAngle;
                }
            } else {
                if (currentAngle + deltaAngle < targetAngle) {
                    deltaAngle = targetAngle - currentAngle;
                }
            }

            transform.Rotate(0, 0, deltaAngle);
            sparkParticleSystem.transform.Rotate(deltaAngle, 0, 0);
            currentAngle += deltaAngle;
            yield return null;
        }
        laserEmitter.PrepareNextState();
        yield return null;
    }

    //private IEnumerator RotateLaserBeam(float next, float angularSpeed, float duration) {
    //    float currentAngle = ((180 + this.transform.eulerAngles.z) % 360) - 180;
    //    targetAngle += next;
    //    targetAngle = ((180 + targetAngle) % 180);

    //    Debug.Log("currentAngle: " + currentAngle);
    //    Debug.Log("targetAngle: " + targetAngle);

    //    float minDelta = targetAngle - currentAngle;
    //    if (Mathf.Abs(minDelta) > ((180 + targetAngle) - currentAngle)) {
    //        minDelta = (180 + targetAngle) - currentAngle;
    //    }
    //    float direction = Mathf.Sign(minDelta);

    //    Debug.Log("minDelta: " + minDelta);
    //    Debug.Log("direction: " + direction);

    //    while (currentAngle != targetAngle) {
    //        float deltaAngle = direction * angularSpeed * Time.deltaTime;
    //        if (direction > 0) {
    //            if (currentAngle + deltaAngle > targetAngle) {
    //                deltaAngle = targetAngle - currentAngle;
    //            }
    //        } else {
    //            if (currentAngle + deltaAngle < targetAngle) {
    //                deltaAngle = targetAngle - currentAngle;
    //            }
    //        }

    //        transform.Rotate(0, 0, deltaAngle);
    //        sparkParticleSystem.transform.Rotate(deltaAngle, 0, 0);
    //        currentAngle = ((180 + this.transform.eulerAngles.z) % 360) - 180;
    //        yield return null;
    //    }
    //    laserEmitter.PrepareNextState();
    //    yield return null;
    //}

    private void LaserWidthAnimation() {
        widthElapsed += Time.deltaTime * 3.0f;
        currentWidth = AppHelper.PingPong(defaultWidth, nextWidth, widthElapsed);
        lineRenderer.widthMultiplier = currentWidth;
    }

    private void LaserLightAnimation() {
        intensityElapsed += Time.deltaTime;
        laserLight.intensity = AppHelper.PingPong(1, 4, intensityElapsed);
    }

    public void SetRotation(float angle) {
        transform.Rotate(0, 0, angle);
    }

    public void SetCurrentState(LaserEmitterScript.LaserEmitterState state) {
        this.state = state;
        stateHandled = false;
        StartCoroutine(HandleState(state.state, state.data));
    }

    //public void LaserRotation(float rotationSpeedMultiplier) {
    //    float angle = Time.deltaTime * rotationSpeedMultiplier;
    //    transform.Rotate(0, 0, angle);
    //    sparkParticleSystem.transform.Rotate(angle, 0, 0);
    //}

    private void CastLaser() {
        //Vector3 direction = target.transform.position - transform.position;
        //RaycastHit2D[] results;
        Vector3 direction = transform.TransformDirection(transform.right);
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, transform.TransformDirection(transform.right), Mathf.Infinity, layerMask);
        if (hit.Length > 1) {
            lineRenderer.SetPosition(1, transform.InverseTransformPoint(hit[1].point));
            laserPointPosition.x = hit[1].point.x;
            laserPointPosition.y = hit[1].point.y;
            laserLight.transform.position = laserPointPosition;
            sparkPosition.x = hit[1].point.x;
            sparkPosition.y = hit[1].point.y;
            sparkParticleSystem.transform.position = sparkPosition;
            CheckIfPlayerWasHit(hit[1]);
        } else {
            lineRenderer.SetPosition(1, transform.right * 100f);
            laserPointPosition.x = transform.right.x * 100f;
            laserPointPosition.y = transform.right.y * 100f;
            laserLight.transform.position = laserPointPosition;
            sparkPosition.x = transform.right.x * 100f;
            sparkPosition.y = transform.right.y * 100f;
            sparkParticleSystem.transform.position = sparkPosition;
        }
        /*
        if (hit) {
            if (hit.collider.gameObject != this) {
                lineRenderer.SetPosition(1, transform.InverseTransformPoint(hit.point));
                laserPointPosition.x = hit.point.x;
                laserPointPosition.y = hit.point.y;
                laserLight.transform.position = laserPointPosition;
                sparkPosition.x = hit.point.x;
                sparkPosition.y = hit.point.y;
                sparkParticleSystem.transform.position = sparkPosition;
                CheckIfPlayerWasHit(hit);
            }
        } else {
            lineRenderer.SetPosition(1, transform.right * 100f);
            laserPointPosition.x = transform.right.x * 100f;
            laserPointPosition.y = transform.right.y * 100f;
            laserLight.transform.position = laserPointPosition;
            sparkPosition.x = transform.right.x * 100f;
            sparkPosition.y = transform.right.y * 100f;
            sparkParticleSystem.transform.position = sparkPosition;
        }
        */
    }

    private Vector2 force = new Vector2(0, -15000);

    private void CheckIfPlayerWasHit(RaycastHit2D hit) {
        if (hit.collider.CompareTag("Player")) {
            if (player == null) {
                player = hit.collider.GetComponentInParent<PlayerController>();
            }
            //player.Knockback(*15000);
            //player.Hurt(0);

            perpendicular.x = transform.TransformDirection(transform.right).y;
            perpendicular.y = -transform.TransformDirection(transform.right).x;

            Vector3 playerVector = player.GetPickupCollider().bounds.center - transform.position;
            float dot = Vector3.Dot(-perpendicular.normalized, playerVector.normalized);

            //Debug.Log(dot);
            //Debug.DrawRay(transform.position, -perpendicular * 10.0f, Color.red, 1.0f);

            if (dot > 0) {
                player.Knockback(-perpendicular * 10000);
            } else {
                player.Knockback(perpendicular * 10000);
            }
            StartCoroutine(player.Hurt(0, 2.0f));
        }
    }

    private void SetParticleEmission(bool value) {
        ParticleSystem.EmissionModule emission = sparkParticleSystem.emission;
        emission.enabled = value;
    }
}
