using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : TogglableObject {

    private Rigidbody2D rb;
    private bool grabbed;
    private Actor currentActor;
    //private Vector2 offset;

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

                //Vector2 size;
                //Vector2 offset;
                Vector3 delta = this.transform.position - currentActor.transform.position;
                float angle = Mathf.Round(this.transform.rotation.eulerAngles.z);
                if (angle == 0) {

                } else if (angle == 90) {

                } else if (angle == 180) {

                } else if (angle == 270) {

                }
                if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y)) {
                    //if (Mathf.Sign(delta.x) < 0) {
                    //    Debug.Log("RIGHT");
                    //    offset.x = 1.0f;
                    //    offset.y = 0.0f;
                    //    size.x = 0.8f;
                    //    size.y = 0.8f;

                    //} else {
                    //    Debug.Log("LEFT");
                    //    offset.x = -1.0f;
                    //    offset.y = 0;
                    //    size.x = 0.8f;
                    //    size.y = 0.8f;
                    //}
                    if (Mathf.Sign(delta.x) < 0) {
                        if (angle == 0) {
                            relativePosition.x = 1.0f;
                        } else if (angle == 90) {
                            relativePosition.y = -1.0f;
                        } else if (angle == 180) {
                            relativePosition.x = -1.0f;
                        } else if (angle == 270) {
                            relativePosition.y = 1.0f;
                        }
                    } else {
                        if (angle == 0) {
                            relativePosition.x = -1.0f;
                        } else if (angle == 90) {
                            relativePosition.y = 1.0f;
                        } else if (angle == 180) {
                            relativePosition.x = 1.0f;
                        } else if (angle == 270) {
                            relativePosition.y = -1.0f;
                        }
                    }
                } else {
                    //if (Mathf.Sign(delta.y) < 0) {
                    //    Debug.Log("TOP");
                    //    offset.x = 0.0f;
                    //    offset.y = 1.0f;
                    //    size.x = 0.8f;
                    //    size.y = 0.8f;
                    //} else {
                    //    Debug.Log("DOWN");
                    //    offset.x = 0.0f;
                    //    offset.y = -1.0f;
                    //    size.x = 0.8f;
                    //    size.y = 0.8f;
                    //}
                    if (Mathf.Sign(delta.y) < 0) {
                        if (angle == 0) {
                            relativePosition.y = 1.0f;
                        } else if (angle == 90) {
                            relativePosition.x = 1.0f;
                        } else if (angle == 180) {
                            relativePosition.y = -1.0f;
                        } else if (angle == 270) {
                            relativePosition.x = -1.0f;
                        }
                    } else {
                        if (angle == 0) {
                            relativePosition.y = -1.0f;
                        } else if (angle == 90) {
                            relativePosition.x = -1.0f;
                        } else if (angle == 180) {
                            relativePosition.y = 1.0f;
                        } else if (angle == 270) {
                            relativePosition.x = 1.0f;
                        }
                    }
                }

                Destroy(rb);

                this.transform.SetParent(actor.transform);
                actor.speed = 3f;
                //delta.x += offset.x;
                //delta.y += offset.y;
                currentActor.UpdateDraggingState(true, relativePosition, this);
            }
        } else {
            //Debug.Log("Object released.");
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
