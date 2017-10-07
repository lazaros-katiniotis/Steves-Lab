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

    private void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
        currentWidth = defaultWidth;
        laserPointPosition = new Vector3(0, 0, -0.05f);
        sparkPosition = new Vector3(0, 0, -0.15f);
    }

    private Vector3 perpendicular;

    void Update() {
        CastLaser();
        LaserWidthAnimation();
        LaserLightAnimation();
    }

    private void LaserWidthAnimation() {
        widthElapsed += Time.deltaTime * 3.0f;
        currentWidth = AppHelper.PingPong(defaultWidth, nextWidth, widthElapsed);
        lineRenderer.widthMultiplier = currentWidth;
    }

    private void LaserLightAnimation() {
        intensityElapsed += Time.deltaTime;
        laserLight.intensity = AppHelper.PingPong(1, 4, intensityElapsed);
    }

    public void LaserRotation(float rotationSpeedMultiplier) {
        float angle = Time.deltaTime * rotationSpeedMultiplier;
        transform.Rotate(0, 0, angle);
        sparkParticleSystem.transform.Rotate(angle, 0, 0);
    }

    private void CastLaser() {
        //Vector3 direction = target.transform.position - transform.position;
        //RaycastHit2D[] results;
        Vector3 direction = transform.TransformDirection(transform.right);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(transform.right), Mathf.Infinity, layerMask);
        if (hit) {
            lineRenderer.SetPosition(1, transform.InverseTransformPoint(hit.point));
            laserPointPosition.x = hit.point.x;
            laserPointPosition.y = hit.point.y;
            laserLight.transform.position = laserPointPosition;
            sparkPosition.x = hit.point.x;
            sparkPosition.y = hit.point.y;
            sparkParticleSystem.transform.position = sparkPosition;
            CheckIfPlayerWasHit(hit);

        } else {
            lineRenderer.SetPosition(1, transform.right * 100f);
            laserPointPosition.x = transform.right.x * 100f;
            laserPointPosition.y = transform.right.y * 100f;
            laserLight.transform.position = laserPointPosition;
            sparkPosition.x = transform.right.x * 100f;
            sparkPosition.y = transform.right.y * 100f;
            sparkParticleSystem.transform.position = sparkPosition;
        }
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
}
