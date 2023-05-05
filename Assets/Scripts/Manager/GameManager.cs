using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    [Header("Caching Components")]
    public Camera mainCamera;

    [Header("State Properties")]
    private EnumsManager.GameState gameState;
    private EnumsManager.LevelState levelState;

    [Header("Gems Properties")]
    [HideInInspector] public int gemsObtainedFromLevel;
    [HideInInspector] public int gemsObtainedFromLeftoverCoin;
    [HideInInspector] public int gemsObtainedFromLeftoverHealth;

    [Header("Time Settings")]
    [SerializeField] private float _playTimeInSeconds;
    public float playTimeInSeconds { get => _playTimeInSeconds; }
    private float _currentTime;
    public float currentTime { get => _currentTime; }

    [Header("Utilities")]
    public GameObject explosionPrefabNormal;
    public GameObject explosionPrefabBlood;
    public bool hasBGMGameplayBeenCalled { get; private set; }

    [Header("GameObject Collector")]
    public List<GameObject> currentEnemies;
    public List<GameObject> currentDecoys;
    public List<GameObject> currentCannons;
    public List<GameObject> currentRoots;
    public List<GameObject> currentThornMine;

    void Awake () {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        SoundManager.Instance.SetVolume(PlayerPrefs.GetFloat(PlayerPrefsKeys.BGM_SLIDER), true);
        SoundManager.Instance.SetVolume(PlayerPrefs.GetFloat(PlayerPrefsKeys.SFX_SLIDER), false);
    }

    void Update() {
        // Updates the game timer and checks the game state to trigger level transitions or game over
        if (IsGameStateGameplay()) {
            _currentTime -= Time.deltaTime;
            if (Mathf.FloorToInt(_currentTime) == Mathf.FloorToInt(playTimeInSeconds) - 7) {
                StartCoroutine(UIManager.Instance.Countdown());
            }
            if (Mathf.FloorToInt(_currentTime) == Mathf.FloorToInt(playTimeInSeconds) - 10 && !hasBGMGameplayBeenCalled) {
                SoundManager.Instance.PlaySound(UtilsClass.GetGameplayBGM());
                hasBGMGameplayBeenCalled = true;
            }
            if (Mathf.FloorToInt(_currentTime) == 20) {
                SoundManager.Instance.StopSound(UtilsClass.GetGameplayBGM());
                hasBGMGameplayBeenCalled = false;
            }
            if ((_currentTime <= 0 && currentEnemies.Count <= 0) || _currentTime <= -20) {
                GamePanelManager.Instance.LevelTransition();

                MissionManager.Instance.UpdateMissionProgress(EnumsManager.Mission.NUMBER_OF_LEVELS_PLAYED);
            }
        }

        EnvironmentManager.Instance.SetGlobalLight();

        // Updates the UI text components
        UIManager.Instance.SetTimerInfo();
        UIManager.Instance.SetLevelInfo();
    }

    // Sets the timer and level state, and starts the corresponding level
    public void ResetGameplay(EnumsManager.LevelState levelState) {
        SetLevelState(levelState);

        for (int i = 0; i < LevelManager.Instance.levels.Count; i++) {
            if (levelState == (EnumsManager.LevelState)i) {
                currentEnemies = new List<GameObject>();
                _playTimeInSeconds = LevelManager.Instance.levels[i].playTimeInSeconds;
                _currentTime = _playTimeInSeconds;
                LevelManager.Instance.StartLevel(LevelManager.Instance.levels[i].enemyAmount);
                EnvironmentManager.Instance.ResetEnvironment(LevelManager.Instance.levels[i]);
                break;
            }
        }
        
    }

    // Adds up all the gems earned and resets the gameplay for the next level
    public void NextLevel() {
        if (GetCurrentLevelState() == LevelManager.Instance.levelStates[LevelManager.Instance.levelStateIndex]) {
            gemsObtainedFromLevel += LevelManager.Instance.levels[LevelManager.Instance.levelStateIndex].gemsObtained;
            SetGameState(EnumsManager.GameState.GAMEPLAY);
            ResetGameplay(LevelManager.Instance.levelStates[LevelManager.Instance.levelStateIndex + 1]);
            LevelManager.Instance.levelStateIndex++;
        }
    }

    // Setters and getters for game and level state
    public void SetGameState(EnumsManager.GameState gameState) => this.gameState = gameState;
    public void SetLevelState(EnumsManager.LevelState levelState) => this.levelState = levelState;
    public EnumsManager.LevelState GetCurrentLevelState() => levelState;

    public bool IsGameStateMainMenu() => gameState == EnumsManager.GameState.MAINMENU;
    public bool IsGameStateGameplay() => gameState == EnumsManager.GameState.GAMEPLAY;
    public bool IsGameStateLevelTransition() => gameState == EnumsManager.GameState.LEVELTRANSITION;
    public bool IsGameStatePause() => gameState == EnumsManager.GameState.PAUSE;

    // Obtain gems from leftover coin and health
    public void SetGemsRewardFromCoinAndHealth() {
        gemsObtainedFromLeftoverCoin = CoinSystem.Instance.GetCurrentCoin();

        // Prevent obtain gems from leftover health at level 1
        if (levelState != EnumsManager.LevelState.LEVEL_1) {
            gemsObtainedFromLeftoverHealth = (int)(
                Player.Instance.healthSystem.currentHealth +
                BanyanDefenseManager.Instance.healthSystem.currentHealth
            );
        }
    }

    // Cheat feature
    public void FastForwardPlayTime(float fastForwardPlayTimeBy) {
        _currentTime -= fastForwardPlayTimeBy;
    }

}

