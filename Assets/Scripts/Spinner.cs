using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour {

    public Vector3 speed;

    public float delay = 0;

    void Start() {
        transform.Rotate(-speed * delay);
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(speed * Time.deltaTime * 100);
	}
}
