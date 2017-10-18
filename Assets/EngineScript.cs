using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineScript : TogglableObject {

    public int generatedEnergy;
    public Transform offSprite;
    public Transform onSprite;

    public override Component GetParticleColliderTransform() {
        throw new NotImplementedException();
    }

    public override void Toggle(Actor actor) {
        activated = !activated;
        if (activated) {
            GameManager.GetInstance().IncreaseLevelEnergy(generatedEnergy);
            offSprite.gameObject.SetActive(false);
            onSprite.gameObject.SetActive(true);
        } else {
            GameManager.GetInstance().DecreaseLevelEnergy(generatedEnergy);

            offSprite.gameObject.SetActive(true);
            onSprite.gameObject.SetActive(false);
        }
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
