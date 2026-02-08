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
        PlayerPrefs.SetFloat("HighScore", TaskManager.Instance.CalculateCompletionPercent());
    }

    // Update is called once per frame
    void Update() {
        if(PlayerUI.instance.paused) {return;}
        currentTime -= Time.deltaTime;
        timerText.text = currentTime.ToString("F1");

        if(currentTime <= 0) {
            EndGame();
        }
    }
}
