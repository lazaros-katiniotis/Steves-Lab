using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour {

    private Rigidbody2D rb;
    //private float collisionElapsed;
    //private float duration = 0.5f;
    //private Vector2 currentPosition;
    //private Vector2 nextPosition;
    //private bool startLerp;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        //currentPosition = rb.position;
        //startLerp = false;
    }

    private void FixedUpdate() {
        rb.MovePosition(rb.position);
        //currentPosition = Vector2.Lerp(currentPosition, nextPosition, 10.0f * Time.deltaTime);
        //rb.MovePosition(currentPosition);
    }

    //private void OnCollisionEnter2D(Collision2D collision) {
    //    //collisionElapsed = 0.0f;
    //    //int i = 0;
    //    //foreach (ContactPoint2D point in collision.contacts) {
    //    //    Debug.Log(i++ + ": " + point.point);
    //    //}
    //}

    //private void OnCollisionStay2D(Collision2D collision) {
    //    //collisionElapsed += Time.deltaTime;
    //    //Debug.Log("Player Position: (" + collision.transform.position.x + ", " + collision.transform.position.y + ")");
    //    // Debug.Log("Box Position: (" + transform.position.x + ", " + transform.position.y + ")");
    //    // Debug.Log("Contact Point: (" + collision.contacts[0].point.x + ", " + collision.contacts[0].point.y + ")");
    //    //if (collisionElapsed >= duration) {
    //    //    collisionElapsed -= duration;
    //    //    //Debug.Log(collision.relativeVelocity);
    //    //    nextPosition = rb.position + collision.relativeVelocity.normalized;

    //    //}
    //}
}
