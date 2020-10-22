using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : TogglableObject {

    private Rigidbody2D rb;
    private bool grabbed;
    private Actor currentActor;

    public InteractionScript interactionScript;

    private void Awake() {
        rb = GetComponentInChildren<Rigidbody2D>();
    }


    public override void Toggle(Actor actor) {
        Vector2 relativePosition = Vector2.zero;
        if (!grabbed) {
            if (!actor.IsDragging()) {
                grabbed = true;
                currentActor = actor;
                currentActor.SetDragging(true);

                Vector3 delta = this.transform.position - currentActor.transform.position;
                float angle = Mathf.Round(this.transform.rotation.eulerAngles.z);

                if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y)) {
                    if (Mathf.Sign(delta.x) < 0) {
                        relativePosition.x = 1.0f;
                    } else {
                        relativePosition.x = -1.0f;
                    }
                } else {
                    if (Mathf.Sign(delta.y) < 0) {
                        relativePosition.y = 1.0f;
                    } else {
                        relativePosition.y = -1.0f;
                    }
                }

                Destroy(rb);
                this.transform.SetParent(actor.transform);

                actor.speed = 3f; // hardcoded
                currentActor.UpdateDraggingState(true, relativePosition, this);
            }
        } else {
            grabbed = false;
            currentActor.SetDragging(false);
            currentActor.UpdateDraggingState(false, relativePosition, null);

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

    public InteractionScript GetInteractionScript() {
        return interactionScript;
    }

}
