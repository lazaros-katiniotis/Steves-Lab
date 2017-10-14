using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawOrderScript : MonoBehaviour {

    private Renderer renderer;

    public string layer;
    public int baseSortingOrder;

    void Start() {
        renderer = GetComponent<Renderer>();
    }

    void LateUpdate() {
        renderer.sortingOrder = baseSortingOrder - (int)(100 * this.transform.position.y);
    }
}
