using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerUI : MonoBehaviour {
    public GameObject pauseMenu;

    public bool paused;

    public static PlayerUI instance;

    private bool endGame = false;
    public GameObject endGameScreen;
    public TMP_Text highscore;
    public TMP_Text thisScore;


    public GameObject egTimesUp;

    public GameObject egRest;

    void Start() {
        instance = this;
        paused = false;        
    }

    public void EndGameScreen() {
        egTimesUp.SetActive(true);
        egRest.SetActive(false);
        endGameScreen.SetActive(true);
        StartCoroutine(EndGameScreenPart2());
    }

    private IEnumerator EndGameScreenPart2() {
        yield return new WaitForSeconds(2f);
        egTimesUp.SetActive(false);
        egRest.SetActive(true);
        endGame = true;
        paused = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        highscore.text = PlayerPrefs.GetFloat("HighScore").ToString();
        thisScore.text = TaskManager.Instance.CalculateCompletionPercent().ToString();
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

        if(Input.GetKeyDown(KeyCode.R) && endGame) {
            SceneManager.LoadScene(1);
        }
        if(endGame) {return;}
        if(Input.GetKeyDown(KeyCode.Escape)) {
            paused = !paused;
            if(paused) {Pause();}
            else {Unpause();}
        }
    }
}
