using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshRendererSorter : MonoBehaviour {

    public string layer;
    public int sortingOrder;

    private void Awake() {
        GetComponent<Renderer>().sortingLayerName = layer;
        GetComponent<Renderer>().sortingOrder = sortingOrder;
    }
}
