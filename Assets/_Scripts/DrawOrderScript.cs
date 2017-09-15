using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawOrderScript : MonoBehaviour {

    private SpriteRenderer spriteRenderer;

    public int baseSortingOrder;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void LateUpdate() {
        spriteRenderer.sortingOrder = baseSortingOrder - (int)(100 * this.transform.position.y);
    }
}
