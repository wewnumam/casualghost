using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private EnumsManager.GameState gameState;
    private EnumsManager.LevelState levelState;
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
        ResetGameplay(EnumsManager.LevelState.LEVEL_1);
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

    public void ResetGameplay(EnumsManager.LevelState levelState) {
        currentTime = playTimeInSeconds;
        SetLevelState(levelState);
        for (int i = 0; i < LevelManager.Instance.levels.Count; i++) {
            if (levelState == (EnumsManager.LevelState)i) {
                LevelManager.Instance.StartLevel(LevelManager.Instance.levels[i].enemyAmount);
                break;
            }
        }
    }

    public void SetGameState(EnumsManager.GameState gameState) {
        this.gameState = gameState;
    }

    public void SetLevelState(EnumsManager.LevelState levelState) {
        this.levelState = levelState;
    }

    public EnumsManager.LevelState GetCurrentLevelState() => levelState;

    public bool IsGameStateMainMenu() => gameState == EnumsManager.GameState.MAINMENU;
    public bool IsGameStateGameplay() => gameState == EnumsManager.GameState.GAMEPLAY;
    public bool IsGameStatePause() => gameState == EnumsManager.GameState.PAUSE;
    public bool IsGameStateGameOver() => gameState == EnumsManager.GameState.GAMEOVER;
    public bool IsGameStateLevelTransition() => gameState == EnumsManager.GameState.LEVELTRANSITION;

    public void SetGemsRewardFromCoinAndHealth() {
        gemsObtainedFromLeftoverCoin = CoinSystem.Instance.GetCurrentCoin();

        if (levelState != EnumsManager.LevelState.LEVEL_1) {
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
        levelInfoText.text = $"{Enum.GetName(typeof(EnumsManager.LevelState), GetCurrentLevelState())} DONE!";
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

