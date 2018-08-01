using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPCTrigger : MonoBehaviour {

    void OnTriggerEnter(Collider other) {
        GameObject go = other.gameObject;
        if (go.tag == "Player") {
            go.GetComponent<PlayerManager>().SetController(false);
            
        }
    }

}
