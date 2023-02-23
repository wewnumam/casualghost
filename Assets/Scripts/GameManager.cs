using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private GameState gameState;
    private LevelState levelState;

    public float playTimeInSeconds;
    private float currentTime;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI levelInfoText;

    void Awake () {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        ResetGameplay(LevelState.LEVEL_1);
    }

    void Update() {
        if (IsGameStateGameplay()) {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0) {
                GamePanelManager.Instance.LevelTransition();
            }
        }
        SetTimerInfo();
        SetLevelInfo();
    }

    public void ResetGameplay(LevelState levelState) {
        currentTime = playTimeInSeconds;
        SetLevelState(levelState);
        if (levelState == LevelState.LEVEL_1) {
            LevelManager.Instance.StartLevel(LevelManager.Instance.levels[0].enemyAmount);
        } else if (levelState == LevelState.LEVEL_2) {
            LevelManager.Instance.StartLevel(LevelManager.Instance.levels[1].enemyAmount);
        }
    }

    public void SetGameState(GameState gameState) {
        this.gameState = gameState;
    }

    public void SetLevelState(LevelState levelState) {
        this.levelState = levelState;
    }

    public LevelState GetCurrentLevelState() => levelState;

    public bool IsGameStateGameplay() => gameState == GameState.GAMEPLAY;
    public bool IsGameStatePause() => gameState == GameState.PAUSE;

    void SetTimerInfo() {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int remainingSeconds = Mathf.FloorToInt(currentTime % 60);

        string timeString = string.Format("{0:00}:{1:00}", minutes, remainingSeconds);
        timerText.text = $"TIMER: {timeString}";
    }

    void SetLevelInfo() {
        levelInfoText.text = $"{Enum.GetName(typeof(LevelState), GetCurrentLevelState())} DONE!";
    }

    // Cheat feature
    public void FastForwardPlayTime(float fastForwardPlayTimeBy) {
        currentTime -= fastForwardPlayTimeBy;
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