using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour {

    public GameObject followedObject;
    private Vector3 delta;
    private float cameraZ;

    // Use this for initialization
    void Start() {
        delta = transform.position - followedObject.transform.position;
        cameraZ = transform.position.z;
    }

    private float time;

    // Update is called once per frame
    void Update() {
        //time += Time.deltaTime;
        //Vector3 start = transform.position - delta;
        //transform.position = Vector3.Lerp(start, followedObject.transform.position, time / 10f);
        //transform.position = new Vector3(transform.position.x, transform.position.y, cameraZ);
    }
}
