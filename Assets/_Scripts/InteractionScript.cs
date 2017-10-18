using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionScript : MonoBehaviour {

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
        GetComponentInParent<GlowableObject>().StartGlow();
        //glowCompositeScript.StartGlow();
        string tag = obj.tag;
        switch (tag) {
            case "Player":
            Debug.Log("Player entered " + this.transform.parent.gameObject + "'s AoI!");
            PlayerController player = obj.GetComponent<PlayerController>();
            player.SetLastInteractedObject(this.transform.parent.gameObject);
            break;
        }
    }

    //private void OnTriggerStay2D(Collider2D collision) {
    //    GetComponentInParent<GlowableObject>().StartGlow();
    //}

    private void OnTriggerExit2D(Collider2D collision) {
        Transform parent = collision.transform.parent;
        if (parent == null) {
            return;
        }
        GameObject obj = parent.gameObject;
        GetComponentInParent<GlowableObject>().EndGlow();
        //glowCompositeScript.EndGlow();
        string tag = obj.tag;
        switch (tag) {
            case "Player":
            Debug.Log("Player exited " + this.transform.parent.gameObject + "'s AoI!");
            PlayerController player = obj.GetComponent<PlayerController>();
            player.SetLastInteractedObject(null);
            break;
        }
        obj = null;
    }
}
