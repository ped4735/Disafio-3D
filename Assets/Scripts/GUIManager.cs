using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour {

    public GameObject win;
    public GameObject lose;

    public PlayerManager player;

    public void Win() {
        win.SetActive(true);
        ActivateCursor();
    }

    public void Lose() {
        lose.SetActive(true);
        ActivateCursor();
    }

    void ActivateCursor() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void DeactivateCursor() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Reset() {
        Debug.Log("Reseting");
        win.SetActive(false);
        lose.SetActive(false);
        player.Reset();
        DeactivateCursor();
    }
}
