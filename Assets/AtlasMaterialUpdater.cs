using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class AtlasMaterialUpdater : MonoBehaviour {

    public void Start() {
        SpriteAtlas atlas = DataManager.GetInstance().GetSpriteAtlas();
        Texture atlasTexture = atlas.GetSprite("floor").texture;
        GetComponent<Renderer>().sharedMaterial.mainTexture = atlasTexture;
    }

}
