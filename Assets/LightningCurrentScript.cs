using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningCurrentScript : MonoBehaviour {

    private List<TogglableObject> affectedObjects;
    private ParticleSystem ps;



    void Start() {
        this.affectedObjects = GetComponentInParent<ActivatorMachineScript>().affectedObjects;
        ps = GetComponent<ParticleSystem>();
        Vector3 direction = this.transform.parent.transform.position - affectedObjects[0].transform.position;
        float distance = Mathf.Sqrt(direction.x * direction.x + direction.y * direction.y);
        var vel = ps.velocityOverLifetime;
        vel.x = -direction.x * 20f / distance;
        vel.y = -direction.y * 20f / distance;


    }

    void Update() {

    }
}
