using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class DataManager : MonoBehaviour {

    public enum TileName {
        floor,
        wall,
        engine_off_placeholder,
        engine_on_placeholder,
        buttonFloor_off_placeholder,
        buttonFloor_on_placeholder,
        wallBottom,
        wallTop,
        flames_0,
        flames_1,
        flames_2,
        box,
        boxCopy,
        doorFrontClosed,
        doorFrontOpen,
        doorIndicator,
        doorIndicatorGreen,
        doorIndicatorRed,
        doorIndicatorYellow,
        doorSideClosed,
        doorSideOpen,
        fence,
        fenceLeft,
        fenceRight,
        healthPack,
        keycard,
        oxygenPack,
        stairsDown,
        stairsLeft,
        stairsRight,
        stairsUp,
        terminal,
        wallAirVent,
        wallLamp,
        wallLamp2
    };

    public class SpriteData {
        private Vector2[] uv;
        private Vector2 spriteScale;

        public SpriteData(Vector2[] uv, Vector2 spriteScale) {
            this.uv = uv;
            this.spriteScale = spriteScale;
        }

        public Vector2[] GetUV() {
            return uv;
        }

        public Vector2 GetSpriteSize() {
            return spriteScale;
        }

    };

    public List<string> scenes;
    public static DataManager instance;
    public SpriteAtlas atlas;
    //private Vector2 atlasSize;

    public AudioClip song;
    private AudioSource audio;
    private float songDelay;

    public int currentLevelIndex;
    private int levelIndex;
    private bool gameJustStarted = true;

    private Dictionary<TileName, SpriteData> spriteData;

    void Start() {
        Debug.Log("DataManager Start()...");
        if (instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
            return;
        }
        levelIndex = currentLevelIndex - 1;
        audio = GetComponent<AudioSource>();
        audio.clip = song;
        audio.loop = true;
        TurnOnMusic(3.45f);
        InitSpriteUVDictionary();
    }

    private void InitSpriteUVDictionary() {
        //Debug.Log("Initing SpriteUV Dictionary...");
        //Texture atlasTexture = atlas.GetSprite("floor").texture;
        //atlasSize = new Vector2(atlasTexture.width, atlasTexture.height);
        spriteData = new Dictionary<TileName, SpriteData>();
        string[] values = Enum.GetNames(typeof(TileName));
        for (int i = 0; i < values.Length; i++) {

            Sprite sprite = atlas.GetSprite(values[i]);
            Rect spriteRect = sprite.textureRect;

            float x = spriteRect.x / sprite.texture.width;
            float y = spriteRect.y / sprite.texture.height;
            float w = (spriteRect.x + spriteRect.width) / sprite.texture.width;
            float h = (spriteRect.y + spriteRect.height) / sprite.texture.height;
            Vector2[] uvs = new Vector2[4];
            uvs[0].Set(x, y);
            uvs[1].Set(w, h);
            uvs[2].Set(w, y);
            uvs[3].Set(x, h);
            Vector2 spriteSize = new Vector2((w - x) * sprite.texture.width / 32, (h - y) * sprite.texture.height / 32);
            SpriteData data = new SpriteData(uvs, spriteSize);
            spriteData.Add((TileName)Enum.Parse(typeof(TileName), values[i]), data);
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
        Debug.Log("Getting next level...");
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

    public SpriteData GetSpriteData(TileName sprite) {
        SpriteData data;
        spriteData.TryGetValue(sprite, out data);
        return data;
    }
}
