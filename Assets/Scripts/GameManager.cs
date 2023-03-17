using System;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    [Header("State Properties")]
    private EnumsManager.GameState gameState;
    private EnumsManager.LevelState levelState;

    [Header("Gems Properties")]
    private int currentGems;
    [HideInInspector] public int gemsObtainedFromLevel;
    [HideInInspector] public int gemsObtainedFromLeftoverCoin;
    [HideInInspector] public int gemsObtainedFromLeftoverHealth;

    [Header("Time Settings")]
    [SerializeField] private float _playTimeInSeconds;
    public float playTimeInSeconds { get => _playTimeInSeconds; }
    private float currentTime;

    [Header("UI Properties")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI[] levelInfoText;
    [SerializeField] private TextMeshProUGUI gemsInfoText;
    [SerializeField] private TextMeshProUGUI rewardInfoText;

    [Header("Environment Properties")]
    [SerializeField] private Light2D directionalLight;
    [SerializeField] private Transform environmentParent;

    void Awake () {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        // Initializes the gem count and starts the first level
        currentGems = PlayerPrefs.GetInt(PlayerPrefsKeys.GEMS);

        SoundManager.Instance.SetVolume(PlayerPrefs.GetFloat(PlayerPrefsKeys.BGM_SLIDER), true);
        SoundManager.Instance.SetVolume(PlayerPrefs.GetFloat(PlayerPrefsKeys.SFX_SLIDER), false);
    }

    void Update() {
        // Updates the game timer and checks the game state to trigger level transitions or game over
        if (IsGameStateGameplay()) {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0) {
                GamePanelManager.Instance.LevelTransition();
            }
        }

        // Updates the UI text components
        SetTimerInfo();
        SetLevelInfo();
        SetGemsInfo();
        SetRewardInfo();
    }

    // Sets the timer and level state, and starts the corresponding level
    public void ResetGameplay(EnumsManager.LevelState levelState) {
        currentTime = _playTimeInSeconds;
        SetLevelState(levelState);

        for (int i = 0; i < LevelManager.Instance.levels.Count; i++) {
            if (levelState == (EnumsManager.LevelState)i) {
                LevelManager.Instance.StartLevel(LevelManager.Instance.levels[i].enemyAmount);
                GameObject environment = Instantiate(LevelManager.Instance.levels[i].environment, environmentParent);
                Destroy(environment, playTimeInSeconds);
                directionalLight.intensity = LevelManager.Instance.levels[i].directionalLightIntensity;
                directionalLight.color = LevelManager.Instance.levels[i].directionalLightColor;
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
    public bool IsGameStatePause() => gameState == EnumsManager.GameState.PAUSE;
    public bool IsGameStateGameOver() => gameState == EnumsManager.GameState.GAMEOVER;
    public bool IsGameStateLevelTransition() => gameState == EnumsManager.GameState.LEVELTRANSITION;

    // Obtain gems from leftover coin and health
    public void SetGemsRewardFromCoinAndHealth() {
        gemsObtainedFromLeftoverCoin = CoinSystem.Instance.GetCurrentCoin();

        // Prevent obtain gems from leftover health at level 1
        if (levelState != EnumsManager.LevelState.LEVEL_1) {
            gemsObtainedFromLeftoverHealth = (int)(
                GameObject.FindWithTag(Tags.PLAYER).GetComponent<HealthSystem>().currentHealth +
                GameObject.FindWithTag(Tags.BANYAN).GetComponent<HealthSystem>().currentHealth
            );
        }
    }
    
    // Adds the total number of gems obtained, saves the to PlayerPrefs
    public void ClaimReward() {
        currentGems += gemsObtainedFromLevel + gemsObtainedFromLeftoverCoin + gemsObtainedFromLeftoverHealth;
        PlayerPrefs.SetInt(PlayerPrefsKeys.GEMS, currentGems);
    }

    void SetTimerInfo() {
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        timerText.text = $"{time:mm\\:ss}";
    }

    void SetLevelInfo() {
        levelInfoText[0].text = $"{Enum.GetName(typeof(EnumsManager.LevelState), GetCurrentLevelState())} DONE!";
        levelInfoText[1].text = $"WAVE: {Enum.GetName(typeof(EnumsManager.LevelState), GetCurrentLevelState())}";
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

