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
    private float currentDirectionalLightIntensity;

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
            currentTime -= Time.deltaTime;
            if (currentTime <= 0) {
                GamePanelManager.Instance.LevelTransition();
            }

            directionalLight.intensity = currentDirectionalLightIntensity;
        } else {
            // Preventing UI from getting too dark
            directionalLight.intensity = 0.8f;
        }

        // Updates the UI text components
        SetTimerInfo();
        SetLevelInfo();
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
                currentDirectionalLightIntensity = LevelManager.Instance.levels[i].directionalLightIntensity;
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

    void SetTimerInfo() {
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        timerText.text = $"{time:mm\\:ss}";
    }

    void SetLevelInfo() {
        levelInfoText[0].text = $"{Enum.GetName(typeof(EnumsManager.LevelState), GetCurrentLevelState())} DONE!";
        levelInfoText[1].text = $"{Enum.GetName(typeof(EnumsManager.LevelState), GetCurrentLevelState())}";
        levelInfoText[2].text = $"Current Level: {Enum.GetName(typeof(EnumsManager.LevelState), GetCurrentLevelState())}";
        levelInfoText[3].text = $"Longest Level: {Enum.GetName(typeof(EnumsManager.LevelState), HighScoreSystem.Instance.currentHighScore)}";
    }

    void SetRewardInfo() {
        rewardInfoText.text = $"Gems x Level: {gemsObtainedFromLevel}\n Gems x Leftover Coin: {gemsObtainedFromLeftoverCoin}\n Gems x Leftover Health: {gemsObtainedFromLeftoverHealth}\n";
    }

    // Cheat feature
    public void FastForwardPlayTime(float fastForwardPlayTimeBy) {
        currentTime -= fastForwardPlayTimeBy;
    }

}

