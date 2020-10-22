using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class MaterialInitializer : MonoBehaviour {

    public DataManager.TileName sprite;

    void Start() {
        DataManager.SpriteData data = DataManager.GetInstance().GetSpriteData(sprite);
        GetComponent<MeshFilter>().mesh.uv = data.GetUV();
        this.transform.localScale = new Vector3(data.GetSpriteSize().x, data.GetSpriteSize().y, 1);
    }

}
