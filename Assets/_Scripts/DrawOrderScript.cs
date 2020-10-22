using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawOrderScript : MonoBehaviour {

    private Renderer renderer;
    public int baseSortingOrder;
    public float scale = 1;

    void Start() {
        renderer = GetComponent<Renderer>();
    }

    void LateUpdate() {
        renderer.sortingOrder = baseSortingOrder - (int)(scale * 100 * this.transform.position.y);
    }
}
