using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineMeshes : MonoBehaviour {

    private void Start() {
        StaticBatchingUtility.Combine(this.gameObject);
    }
}
