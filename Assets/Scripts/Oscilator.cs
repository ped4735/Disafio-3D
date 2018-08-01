using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscilator : MonoBehaviour {

    public enum OscilationType {
        NONE, SIN, COS, MOD
    }

    public OscilationType xOscilation = OscilationType.NONE;
    public float xMagnitude = 10;
    public float xInterval = 1;

    public OscilationType yOscilation = OscilationType.NONE;
    public float yMagnitude = 10;
    public float yInterval = 1;

    public OscilationType zOscilation = OscilationType.NONE;
    public float zMagnitude = 10;
    public float zInterval = 1;

    public bool useRigidbody = false;
    Rigidbody rb;

    Vector3 origin;

    public bool fixCollision = false;

    public float delay = 0;

    float extent;
    float playerExtent;
    GameObject player;

    // Use this for initialization
    void Start () {
        origin = transform.position;
        rb = GetComponent<Rigidbody>();
        extent = GetComponent<Collider>().bounds.extents.y;

    }
	
	// Update is called once per frame
	void Update () {
        float time = Time.time + delay;

        // X
        float dx = 0;
        switch (xOscilation) {
            case OscilationType.NONE:
                break;
            case OscilationType.SIN:
                dx = Mathf.Sin(time * Mathf.PI * 2 / xInterval) * xMagnitude;
                break;
            case OscilationType.COS:
                dx = Mathf.Cos(time * Mathf.PI * 2 / xInterval) * xMagnitude;
                break;
            case OscilationType.MOD:
                dx = (time % xInterval) * xMagnitude;
                break;
        }

        // Y
        float dy = 0;
        switch (yOscilation) {
            case OscilationType.NONE:
                break;
            case OscilationType.SIN:
                dy = Mathf.Sin(time * Mathf.PI * 2 / yInterval) * yMagnitude;
                break;
            case OscilationType.COS:
                dy = Mathf.Cos(time * Mathf.PI * 2 / yInterval) * yMagnitude;
                break;
            case OscilationType.MOD:
                dy = (time % yInterval) * yMagnitude;
                break;
        }

        // Z
        float dz = 0;
        switch (zOscilation) {
            case OscilationType.NONE:
                break;
            case OscilationType.SIN:
                dz = Mathf.Sin(time * Mathf.PI * 2 / zInterval) * zMagnitude;
                break;
            case OscilationType.COS:
                dz = Mathf.Cos(time * Mathf.PI * 2 / zInterval) * zMagnitude;
                break;
            case OscilationType.MOD:
                dz = (time % zInterval) * zMagnitude;
                break;
        }

        Vector3 vary = new Vector3(dx, dy, dz);
        Vector3 newPos = origin + vary;

        // Set Position
        if (useRigidbody) {
            rb.MovePosition(newPos);
        } else {
            transform.position = newPos;
        }

        // Fix player collision
        if (fixCollision && player != null) {
            player.transform.Translate(vary);
        }
    }

    void OnCollisionEnter(Collision collision) {
        GameObject go = collision.gameObject;
        if(go.tag == "Player") {
            player = go;
        }
    }

    void OnCollisionExit(Collision collision) {
        if(collision.gameObject.tag == "Player") {
            player = null;
        }
    }
}
