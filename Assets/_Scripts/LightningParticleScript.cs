using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LightningParticleScript : MonoBehaviour {

    private List<TogglableObject> affectedObjects;
    private ParticleSystem ps;
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
    private Vector3 particleVelocity = new Vector3(0, 0, 0);
    private Color32 finalColor;

    void Start() {


    }

    void OnEnable() {
        ActivatorMachineScript script = GetComponentInParent<ActivatorMachineScript>();
        this.affectedObjects = script.affectedObjects;
        ps = GetComponent<ParticleSystem>();
        Vector3 direction = this.transform.parent.transform.position - affectedObjects[0].transform.position;
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
        main.startLifetime = (distance / velocity) + 1;

        var trigger = ps.trigger;
        trigger.SetCollider(0, script.affectedObjects[0].GetParticleColliderTransform());
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

    }
}
