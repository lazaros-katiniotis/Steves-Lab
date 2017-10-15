﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor.Sprites;
using UnityEngine;
using UnityEngine.U2D;

public class MaterialInitializer : MonoBehaviour {

    public string spriteAtlasName;
    public string spriteName;

    //Create material with texture atlas using this:
    //Texture tex = SpriteUtility.GetSpriteTexture(sprite, true);
    //GetComponent<Renderer>().material.mainTexture = tex;

    void Start() {
        SpriteAtlas atlas = DataManager.GetInstance().GetSpriteAtlas(spriteAtlasName);

        Sprite sprite = atlas.GetSprite(spriteName);
        Rect spriteRect = sprite.textureRect;

        float x = spriteRect.x / sprite.texture.width;
        float y = spriteRect.y / sprite.texture.height;
        float w = (spriteRect.x + spriteRect.width) / sprite.texture.width;
        float h = (spriteRect.y + spriteRect.height) / sprite.texture.height;

        Vector2[] newUvs = new Vector2[4];
        newUvs[0].Set(x, y);
        newUvs[1].Set(w, h);
        newUvs[2].Set(w, y);
        newUvs[3].Set(x, h);

        GetComponent<MeshFilter>().mesh.uv = newUvs;
    }

}
