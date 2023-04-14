using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using Cinemachine;

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
    [SerializeField] private TooltipTrigger timerTooltip;
    [SerializeField] private TextMeshProUGUI[] levelInfoText;
    [SerializeField] private TooltipTrigger levelTooltip;
    [SerializeField] private TextMeshProUGUI gemsInfoText;
    [SerializeField] private TextMeshProUGUI rewardInfoText;
    [SerializeField] private Slider levelProgress;
    [SerializeField] private Slider timeProgress;
    [SerializeField] private TextMeshProUGUI countdownText;

    [Header("Environment Properties")]
    [SerializeField] private Light2D directionalLight;
    [SerializeField] private Transform environmentParent;
    private float currentDirectionalLightIntensity;
    [SerializeField] private Light2D[] UILights;
    private float[] UILightsIntensity;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    private Coroutine zoomRoutine;
    public bool hasBGMGameplayBeenCalled { get; private set; }

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

        UILightsIntensity = new float[UILights.Length];
        for (int i = 0; i < UILights.Length; i++) {
            UILightsIntensity[i] = UILights[i].intensity;
        }
    }
    void Update() {
        // Updates the game timer and checks the game state to trigger level transitions or game over
        if (IsGameStateGameplay()) {
            currentTime -= Time.deltaTime;
            if (Mathf.FloorToInt(currentTime) == Mathf.FloorToInt(playTimeInSeconds) - 7) {
                StartCoroutine(Countdown(countdownText));
            }
            if (Mathf.FloorToInt(currentTime) == Mathf.FloorToInt(playTimeInSeconds) - 10 && !hasBGMGameplayBeenCalled) {
                SoundManager.Instance.PlaySound(EnumsManager.SoundEffect._BGM_GAMEPLAY_1);
                hasBGMGameplayBeenCalled = true;
            }
            if (Mathf.FloorToInt(currentTime) == 20) {
                SoundManager.Instance.StopSound(EnumsManager.SoundEffect._BGM_GAMEPLAY_1);
                hasBGMGameplayBeenCalled = false;
            }
            if ((currentTime <= 0 && GameObject.FindGameObjectsWithTag(Tags.ENEMY).Length <= 0) || currentTime <= -20) {
                GamePanelManager.Instance.LevelTransition();
            }
        }

        SetLights();

        // Updates the UI text components
        SetTimerInfo();
        SetLevelInfo();
        SetRewardInfo();
    }

    // Sets the timer and level state, and starts the corresponding level
    public void ResetGameplay(EnumsManager.LevelState levelState) {
        SetLevelState(levelState);

        for (int i = 0; i < LevelManager.Instance.levels.Count; i++) {
            if (levelState == (EnumsManager.LevelState)i) {
                _playTimeInSeconds = LevelManager.Instance.levels[i].playTimeInSeconds;
                currentTime = _playTimeInSeconds;
                LevelManager.Instance.StartLevel(LevelManager.Instance.levels[i].enemyAmount);
                GameObject environment = Instantiate(LevelManager.Instance.levels[i].environment, environmentParent);
                Destroy(environment, playTimeInSeconds);
                currentDirectionalLightIntensity = LevelManager.Instance.levels[i].directionalLightIntensity;
                directionalLight.intensity = LevelManager.Instance.levels[i].directionalLightIntensity;
                directionalLight.color = LevelManager.Instance.levels[i].directionalLightColor;
                if (zoomRoutine != null) StopCoroutine(zoomRoutine);
                zoomRoutine = StartCoroutine(CameraZoom(LevelManager.Instance.levels[i].cameraOrthoSize));
                break;
            }
        }
        
    }

    IEnumerator CameraZoom(float targetSize) {
        yield return new WaitForSeconds(2f);
        const float MAX_ORTHOGRAPHIC_SIZE = 50;
        float elapsedTime = 0f;

        while (elapsedTime < 1f) {
            cinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(targetSize, MAX_ORTHOGRAPHIC_SIZE, elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        while (elapsedTime > 0) {
            cinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(targetSize, MAX_ORTHOGRAPHIC_SIZE, elapsedTime);
            elapsedTime -= Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator Countdown(TextMeshProUGUI textMesh) {
        textMesh.gameObject.SetActive(true);
        textMesh.text = "READY";
        yield return new WaitForSeconds(2f);
        textMesh.text = "3";
        yield return new WaitForSeconds(1f);
        textMesh.text = "2";
        yield return new WaitForSeconds(1f);
        textMesh.text = "1";
        yield return new WaitForSeconds(1f);
        textMesh.gameObject.SetActive(false);
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
        timeProgress.maxValue = playTimeInSeconds;
        timeProgress.value = currentTime;
        timerTooltip.header = $"{time:mm\\:ss}";
    }

    void SetLevelInfo() {
        levelInfoText[0].text = $"{Enum.GetName(typeof(EnumsManager.LevelState), GetCurrentLevelState())} DONE!";
        levelInfoText[1].text = $"{Enum.GetName(typeof(EnumsManager.LevelState), GetCurrentLevelState())}";
        levelInfoText[2].text = $"Current Level: {Enum.GetName(typeof(EnumsManager.LevelState), GetCurrentLevelState())}";
        if (GetCurrentLevelState() > HighScoreSystem.Instance.currentHighScore) {
            levelInfoText[3].text = $"Longest Level: {Enum.GetName(typeof(EnumsManager.LevelState), GetCurrentLevelState())}";
            levelInfoText[4].gameObject.SetActive(true);
        } else {
            levelInfoText[3].text = $"Longest Level: {Enum.GetName(typeof(EnumsManager.LevelState), HighScoreSystem.Instance.currentHighScore)}";
            levelInfoText[4].gameObject.SetActive(false);
        }
        levelProgress.maxValue = LevelManager.Instance.levelStates.Length;
        levelProgress.value = (int)GetCurrentLevelState() + 1;
        levelTooltip.header = $"{Enum.GetName(typeof(EnumsManager.LevelState), GetCurrentLevelState())}";
    }

    void SetRewardInfo() {
        rewardInfoText.text = $"Level:    {gemsObtainedFromLevel,3}\nLeftover Coin:    {gemsObtainedFromLeftoverCoin,3}\nLeftover Health:    {gemsObtainedFromLeftoverHealth,3}\n";
    }

    public void SetLights() {
        if (IsGameStateGameplay()) {
            directionalLight.intensity = currentDirectionalLightIntensity;
        } else {
            // Preventing UI from getting too dark
            directionalLight.intensity = 0.8f;
        }
    }

    public void TurnOnUILights(bool isTurnOn) {
        if (isTurnOn) {
            for (int i = 0; i < UILights.Length; i++) {
                UILights[i].intensity = UILightsIntensity[i];
            }
        } else {
            for (int i = 0; i < UILights.Length; i++) {
                UILights[i].intensity = 0;
            }
        }
    }

    // Cheat feature
    public void FastForwardPlayTime(float fastForwardPlayTimeBy) {
        currentTime -= fastForwardPlayTimeBy;
    }

}

