using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float playTimeInSeconds;
    private float currentTime;
    [SerializeField] private TextMeshProUGUI timerText;

    void Start() {
        currentTime = playTimeInSeconds;    
    }

    void Update() {
        if (GamePanelManager.Instance.GetGameState() == GameState.GAMEPLAY) {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0) {
                GamePanelManager.Instance.LevelTransition();
            }
        }
        SetTimerInfo();
    }

    void SetTimerInfo() {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int remainingSeconds = Mathf.FloorToInt(currentTime % 60);

        string timeString = string.Format("{0:00}:{1:00}", minutes, remainingSeconds);
        timerText.text = $"TIMER: {timeString}";
    }

}
