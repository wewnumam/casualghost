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
    private int currentGems;
    [HideInInspector] public int gemsObtainedFromLevel;
    [HideInInspector] public int gemsObtainedFromLeftoverCoin;
    [HideInInspector] public int gemsObtainedFromLeftoverHealth;

    public float playTimeInSeconds;
    private float currentTime;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI levelInfoText;
    [SerializeField] private TextMeshProUGUI gemsInfoText;
    [SerializeField] private TextMeshProUGUI rewardInfoText;

    void Awake () {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        currentGems = PlayerPrefs.GetInt(PlayerPrefsKeys.GEMS);
        ResetGameplay(LevelState.LEVEL_1);
    }

    void Update() {
        if (IsGameStateGameplay()) {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0) {
                GamePanelManager.Instance.LevelTransition();
            }
        } else if (IsGameStateGameOver()) {
            
        }

        SetTimerInfo();
        SetLevelInfo();
        SetGemsInfo();
        SetRewardInfo();
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

    public bool IsGameStateMainMenu() => gameState == GameState.MAINMENU;
    public bool IsGameStateGameplay() => gameState == GameState.GAMEPLAY;
    public bool IsGameStatePause() => gameState == GameState.PAUSE;
    public bool IsGameStateGameOver() => gameState == GameState.GAMEOVER;

    public void SetGemsRewardFromCoinAndHealth() {
        gemsObtainedFromLeftoverCoin = CoinSystem.Instance.GetCurrentCoin();

        if (levelState != LevelState.LEVEL_1) {
            gemsObtainedFromLeftoverHealth = (int)(
                GameObject.FindWithTag(Tags.PLAYER).GetComponent<HealthSystem>().currentHealth +
                GameObject.FindWithTag(Tags.BANYAN).GetComponent<HealthSystem>().currentHealth
            );
        }
    }
    
    public void ClaimReward() {
        currentGems += gemsObtainedFromLevel + gemsObtainedFromLeftoverCoin + gemsObtainedFromLeftoverHealth;
        PlayerPrefs.SetInt(PlayerPrefsKeys.GEMS, currentGems);
    }

    void SetTimerInfo() {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int remainingSeconds = Mathf.FloorToInt(currentTime % 60);

        string timeString = string.Format("{0:00}:{1:00}", minutes, remainingSeconds);
        timerText.text = $"TIMER: {timeString}";
    }

    void SetLevelInfo() {
        levelInfoText.text = $"{Enum.GetName(typeof(LevelState), GetCurrentLevelState())} DONE!";
    }

    void SetGemsInfo() {
        gemsInfoText.text = $"GEMS: {PlayerPrefs.GetInt(PlayerPrefsKeys.GEMS)}";
    }

    void SetRewardInfo() {
        rewardInfoText.text = $"Gems x Level: {gemsObtainedFromLevel}\n Gems x Leftover Coin: {gemsObtainedFromLeftoverCoin}\n Gems x Leftover Health: {gemsObtainedFromLeftoverHealth}\n";
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