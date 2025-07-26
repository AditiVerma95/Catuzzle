using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {
    [SerializeField] private GameObject gameOverPanel;

    [SerializeField] private TextMeshProUGUI userStepsText;
    [SerializeField] private TextMeshProUGUI userTimeText;
    
    [SerializeField] private TextMeshProUGUI highScoreStepsText;
    [SerializeField] private TextMeshProUGUI highScoreTimeText;

    

    public void StartGameOver(int userSteps, float userTime) {
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
        
        userStepsText.text = "Your Steps\n" + userSteps;
        userTimeText.text = "Your Time\n" + ConvertSecondsToTimeFormat((int)userTime);
    }
    
    public string ConvertSecondsToTimeFormat(int totalSeconds) {
        TimeSpan time = TimeSpan.FromSeconds(totalSeconds);
        return string.Format("{0:D2}:{1:D2}:{2:D2}", time.Hours, time.Minutes, time.Seconds);
    }

    public void Restart() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void GoToMainMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
