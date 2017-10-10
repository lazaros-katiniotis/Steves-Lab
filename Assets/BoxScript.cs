using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : TogglableObject {

    private Rigidbody2D rb;
    private bool grabbed;
    private Actor currentActor;
    private Vector2 offset;

    public override void Toggle(Actor actor) {
        grabbed = !grabbed;
        if (grabbed) {
            Debug.Log("Object grabbed!");
            currentActor = actor;
            offset = transform.position - currentActor.transform.position;
            currentActor.UpdateDraggingState(true, this);

        } else {
            Debug.Log("Object released.");
            currentActor.UpdateDraggingState(false, null);
            currentActor = null;
        }
    }

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        if (grabbed) {
            rb.MovePosition((Vector2)currentActor.transform.position + offset);
        } else {
            rb.MovePosition(rb.position);
        }
    }

    public override Component GetParticleColliderTransform() {
        throw new NotImplementedException();
    }

}
