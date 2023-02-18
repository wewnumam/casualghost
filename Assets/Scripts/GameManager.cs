using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private GameState gameState;

    [SerializeField] private float playTimeInSeconds;
    private float currentTime;
    [SerializeField] private TextMeshProUGUI timerText;

    void Awake () {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        currentTime = playTimeInSeconds;    
    }

    void Update() {
        if (IsGameStateGameplay()) {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0) {
                GamePanelManager.Instance.LevelTransition();
            }
        }
        SetTimerInfo();
    }

    public void SetGameState(GameState gameState) {
        this.gameState = gameState;
    }

    public bool IsGameStateGameplay() => gameState == GameState.GAMEPLAY;
    public bool IsGameStatePause() => gameState == GameState.PAUSE;

    void SetTimerInfo() {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int remainingSeconds = Mathf.FloorToInt(currentTime % 60);

        string timeString = string.Format("{0:00}:{1:00}", minutes, remainingSeconds);
        timerText.text = $"TIMER: {timeString}";
    }

}

public enum GameState {
    MAINMENU,
    GAMEPLAY,
    PAUSE,
    LEVELTRANSITION,
    GAMEOVER,
    REWARD
}