using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameTrigger : MonoBehaviour {

    public GUIManager gui;

	void OnTriggerEnter(Collider other) {
        GameObject go = other.gameObject;
        if (go.tag == "Player") {
            go.GetComponent<PlayerManager>().won = true;
            gui.Win();
        }
    }
}
