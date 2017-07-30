using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemDrawOrderScript : MonoBehaviour {

    private Renderer renderer;

    // Use this for initialization
    void Start() {
        renderer = GetComponent<ParticleSystem>().GetComponent<Renderer>();
        renderer.sortingLayerName = "Walls";
    }

    void LateUpdate() {
        renderer.sortingOrder = (int)(10 * this.transform.position.y);

    }
}
