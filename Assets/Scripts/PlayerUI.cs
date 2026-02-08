using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour {
    public GameObject pauseMenu;

    public bool paused;

    public static PlayerUI instance;

    public GameObject endGameScreen;

    void Start() {
        instance = this;
        paused = false;        
    }

    private void Pause() {
        pauseMenu.SetActive(true);
        paused = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    private void Unpause() {
        pauseMenu.SetActive(false);
        paused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            paused = !paused;
            if(paused) {Pause();}
            else {Unpause();}
        }
    }
}
