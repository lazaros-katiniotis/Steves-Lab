using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RendererSorter : MonoBehaviour {

    public string layer;
    public int order;

    private Renderer renderer;

    private void Awake() {
        renderer = GetComponent<Renderer>();
        renderer.sortingLayerName = layer;
        renderer.sortingOrder = order;
    }
}
