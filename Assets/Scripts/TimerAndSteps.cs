using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerAndSteps : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI stepsText;

    public bool isTimerStarted = false;
    public float elapsedTime = 0f;

    public bool isStepsStarted = false;
    public int steps = 0;

    private void Update() {
        if (isTimerStarted) {
            UpdateTimer();
        }
    }

    public void UpdateSteps() {
        if (!isStepsStarted) return;
        stepsText.text = "" + ++steps;
    }
    
    public string ConvertSecondsToTimeFormat(int totalSeconds) {
        TimeSpan time = TimeSpan.FromSeconds(totalSeconds);
        return string.Format("{0:D2}:{1:D2}:{2:D2}", time.Hours, time.Minutes, time.Seconds);
    }
    
    public void UpdateTimer() {
        // Update elapsed time
        elapsedTime += Time.deltaTime;

        // Convert to hours, minutes, seconds
        int hours = Mathf.FloorToInt(elapsedTime / 3600f);
        int minutes = Mathf.FloorToInt((elapsedTime % 3600f) / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        // Format as 00:00:00
        timerText.text = ConvertSecondsToTimeFormat((int) elapsedTime);
        //timerText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }

}
