using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class MaterialInitializer : MonoBehaviour {

    public DataManager.TileName sprite;

    void Start() {
        GetComponent<MeshFilter>().mesh.uv = DataManager.GetInstance().GetSpriteUV(sprite);
    }

}
