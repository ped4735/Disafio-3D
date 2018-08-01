using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour {

    public Follow camera;
    public GameObject node;
    public PlayerManager pm;

    void OnTriggerEnter(Collider collider) {
        GameObject go = collider.gameObject;
        if(go.tag == "Player") {
            camera.follows.Add(node);
            pm.disableRotation = true;
        }
    }

    void OnTriggerExit(Collider collider) {
        GameObject go = collider.gameObject;
        if(go.tag == "Player") {
            camera.follows.Remove(node);
            pm.disableRotation = false;
        }
    }
	
}
