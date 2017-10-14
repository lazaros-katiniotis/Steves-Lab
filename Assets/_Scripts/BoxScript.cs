using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : TogglableObject {

    private Rigidbody2D rb;
    private bool grabbed;
    private Actor currentActor;
    //private Vector2 offset;

    private void Awake() {
        rb = GetComponentInChildren<Rigidbody2D>();
    }


    public override void Toggle(Actor actor) {
        if (!grabbed) {
            if (!actor.IsDragging()) {
                //Debug.Log("Object grabbed!");
                grabbed = true;
                currentActor = actor;
                currentActor.SetDragging(true);
                //offset = transform.position - currentActor.transform.position;
                Destroy(rb);
                this.transform.SetParent(actor.transform);
                actor.speed = 3f;
                currentActor.UpdateDraggingState(true, this);
            }
        } else {
            //Debug.Log("Object released.");
            grabbed = false;
            currentActor.SetDragging(false);
            currentActor.UpdateDraggingState(false, null);
            CreateRigidbody();
            this.transform.SetParent(actor.GetCurrentRoom().GetObjects());
            actor.speed = 4f;
            currentActor = null;
        }
    }

    private void CreateRigidbody() {
        rb = this.gameObject.AddComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        rb.gravityScale = 0.0f;
        rb.mass = 10.0f;
    }

    private void FixedUpdate() {
        if (!grabbed) {
            rb.MovePosition(rb.position);
        }
    }

    public override Component GetParticleColliderTransform() {
        throw new NotImplementedException();
    }

}
