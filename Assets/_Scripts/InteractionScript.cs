using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionScript : MonoBehaviour {

    private enum BoxColliderId : int {
        UP, LEFT, DOWN, RIGHT
    }

    private GlowComposite glowCompositeScript;

    private void Awake() {
        glowCompositeScript = Camera.main.GetComponent<GlowComposite>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Transform parent = collision.transform.parent;
        if (parent == null) {
            return;
        }

        GameObject obj = parent.gameObject;
        GlowableObject glowableObj = GetComponentInParent<GlowableObject>();
        if (glowableObj != null) {
            GetComponentInParent<GlowableObject>().StartGlow();
        }
        string tag = obj.tag;
        switch (tag) {
            case "Player":
            Debug.Log("Player entered " + this.transform.parent.gameObject + "'s AoI!");
            PlayerController player = obj.GetComponent<PlayerController>();
            player.SetLastInteractableObjectInRange(this.transform.parent.gameObject);
            break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        Transform parent = collision.transform.parent;
        if (parent == null) {
            return;
        }
        GameObject obj = parent.gameObject;
        GlowableObject glowableObj = GetComponentInParent<GlowableObject>();
        if (glowableObj != null) {
            GetComponentInParent<GlowableObject>().EndGlow();
        }
        string tag = obj.tag;
        switch (tag) {
            case "Player":
            Debug.Log("Player exited " + this.transform.parent.gameObject + "'s AoI!");
            PlayerController player = obj.GetComponent<PlayerController>();
            player.SetLastInteractableObjectInRange(null);
            break;
        }
        obj = null;
    }
}
