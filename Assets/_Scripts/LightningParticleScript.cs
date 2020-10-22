using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LightningParticleScript : MonoBehaviour {

    private List<TogglableObject> affectedObjects;
    private ParticleSystem ps;
    private List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
    private Vector3 particleVelocity = new Vector3(0, 0, 0);
    private Color32 finalColor;

    private TerminalScript terminalScript;

    void Start() {


    }

    void OnEnable() {

    }

    public void Init(TerminalScript terminalScript, int affectedObjectIndex) {
        this.affectedObjects = terminalScript.affectedObjects;
        this.terminalScript = terminalScript;
        Vector3 direction;
        ps = GetComponent<ParticleSystem>();
        if (affectedObjects.Count != 0) {
            direction = this.transform.parent.transform.position - affectedObjects[affectedObjectIndex].transform.position;
        } else {
            direction = Vector3.up;
        }
        float distance = Mathf.Sqrt(direction.x * direction.x + direction.y * direction.y);
        var vel = ps.velocityOverLifetime;
        vel.x = -direction.x * 20f / distance;
        vel.y = -direction.y * 20f / distance;
        var main = ps.main;
        float speed = 20f;
        particleVelocity.x = direction.x * speed / distance;
        particleVelocity.y = direction.y * speed / distance;
        particleVelocity.z = 0;
        float velocity = Mathf.Sqrt(particleVelocity.x * particleVelocity.x + particleVelocity.y * particleVelocity.y);
        main.startLifetime = (0.5f*distance / velocity) + 1;

        ParticleSystem.Burst[] bursts = new ParticleSystem.Burst[1];
        bursts[0] = new ParticleSystem.Burst(0.0f, 1, 1, 0, Random.Range(3.25f, 3.75f));
        ps.emission.SetBursts(bursts);

        var trigger = ps.trigger;
        trigger.SetCollider(0, terminalScript.affectedObjects[affectedObjectIndex].GetParticleColliderTransform());
    }


    void OnParticleTrigger() {
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        // iterate through the particles which exited the trigger and make them green
        for (int i = 0; i < numEnter; i++) {
            ParticleSystem.Particle p = enter[i];
            //p.startColor = new Color32(0, 255, 255, 255);
            p.velocity = particleVelocity;
            p.angularVelocity = 0;
            p.remainingLifetime = 0.25f;
            enter[i] = p;
        }
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
    }

    void Update() {
        if (terminalScript != null) {
            if (!terminalScript.IsActivated()) {
                if (ps.particleCount == 0) {
                    this.gameObject.SetActive(false);
                }
            }
        }
    }
}
