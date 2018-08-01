using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public int damage = 1;

    void OnCollisionEnter(Collision collision) {
        GameObject go = collision.gameObject;
        if (go.tag == "Player") {
            Debug.Log("inflicting " + damage + " damage");
            go.GetComponent<PlayerManager>().Damage(damage);
        }
    }
}
