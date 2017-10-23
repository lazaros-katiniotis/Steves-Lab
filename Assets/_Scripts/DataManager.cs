using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class DataManager : MonoBehaviour {

    public List<string> scenes;
    public static DataManager instance;
    public SpriteAtlas atlas;

    public AudioClip song;
    private AudioSource audio;
    private float songDelay;

    private int levelIndex = 0;
    private bool gameJustStarted = true;

    private Dictionary<TileName, Vector2[]> spriteUvs;

    public enum TileName {
        floor,
        wall,
        engine_off_placeholder,
        engine_on_placeholder,
        buttonFloor_off_placeholder,
        buttonFloor_on_placeholder,
        wallBottom,
        wallTop,
    };

    void Start() {
        Debug.Log("DataManager Start()...");
        if (instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
            return;
        }

        audio = GetComponent<AudioSource>();
        audio.clip = song;
        audio.loop = true;
        TurnOnMusic(3.45f);
        InitSpriteUVDictionary();
    }

    //Create material with texture atlas using this:
    //Texture tex = SpriteUtility.GetSpriteTexture(sprite, true);
    //GetComponent<Renderer>().material.mainTexture = tex;

    private void InitSpriteUVDictionary() {
        //Debug.Log("Initing SpriteUV Dictionary...");
        spriteUvs = new Dictionary<TileName, Vector2[]>();
        string[] values = Enum.GetNames(typeof(TileName));
        for (int i = 0; i < values.Length; i++) {

            Sprite sprite = atlas.GetSprite(values[i]);
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

            spriteUvs.Add((TileName)Enum.Parse(typeof(TileName), values[i]), newUvs);
        }
    }

    public void TurnOnMusic(float delay) {
        audio.volume = 0.65f;
        audio.PlayDelayed(delay);
    }

    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    void Update() {

    }

    public static DataManager GetInstance() {
        return instance;
    }

    public string GetCurrentScene() {
        return scenes[levelIndex];
    }

    public void NextLevel() {
        levelIndex++;
        UnityEngine.SceneManagement.SceneManager.LoadScene(GetCurrentScene());
    }

    public bool HasGameJustStarted() {
        return gameJustStarted;
    }

    public void SetGameJustStarted(bool value) {
        gameJustStarted = value;
    }

    public SpriteAtlas GetSpriteAtlas() {
        return atlas;
    }

    public Vector2[] GetSpriteUV(TileName sprite) {
        Vector2[] array;
        spriteUvs.TryGetValue(sprite, out array);
        return array;
    }
}
