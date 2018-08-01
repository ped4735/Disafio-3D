using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour {

    public List<GameObject> follows;
    public float speed = 1;
    public bool rotate = true;
    public Vector3Int axis = new Vector3Int(1, 1, 1);

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GameObject followed = follows[follows.Count - 1];
        Vector3 curr = transform.position;
        Vector3 newPos = followed.transform.position;

        newPos = Treat(newPos, curr, axis);

        transform.position = Vector3.Lerp(curr, newPos, Time.deltaTime * speed);
        if (rotate)
            transform.rotation = Quaternion.Slerp(transform.rotation, followed.transform.rotation, Time.deltaTime * speed);
	}

    Vector3 Mult(Vector3 a, Vector3 b) {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }

    Vector3 Treat(Vector3 a, Vector3 b, Vector3Int filter) {
        return Mult(filter, a) + Mult(Vector3Int.one - filter, b);
    }
}
