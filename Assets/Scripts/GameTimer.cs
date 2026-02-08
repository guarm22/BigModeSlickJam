using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour{

    public float startTime;

    public TMP_Text timerText;

    private float currentTime;

    void Start() {
        currentTime = startTime;
    }

    private void EndGame() {
        float hs = PlayerPrefs.GetFloat("HighScore", TaskManager.Instance.CalculateCompletionPercent());
        if(hs < TaskManager.Instance.CalculateCompletionPercent()) {PlayerPrefs.SetFloat("HighScore", TaskManager.Instance.CalculateCompletionPercent());}

        PlayerUI.instance.EndGameScreen();
    }

    // Update is called once per frame
    void Update() {
        if(PlayerUI.instance.paused) {return;}
        currentTime -= Time.deltaTime;
        timerText.text = currentTime.ToString("F1");

        if(currentTime <= 0) {
            currentTime = 0f;
            EndGame();
        }
    }
}
