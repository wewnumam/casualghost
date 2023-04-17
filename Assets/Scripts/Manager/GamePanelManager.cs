using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePanelManager : MonoBehaviour {
    public static GamePanelManager Instance { get; private set; }

    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gameplayPanel;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject levelTransitionPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject rewardPanel;
    [SerializeField] private GameObject optionMenuPanel;
    [SerializeField] private GameObject lastLevelPanel;
    [SerializeField] private GameObject loadingPanel;

    private bool isInventoryOpen = true;
    

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        mainMenuPanel.SetActive(true);
        if (mainMenuPanel.activeInHierarchy) {
            GameManager.Instance.SetGameState(EnumsManager.GameState.MAINMENU);
            GameManager.Instance.TurnOnUILights(false);
            Time.timeScale = 0f;
            SoundManager.Instance.StopSound(EnumsManager.SoundEffect._AMBIENCE_FOREST);
            SoundManager.Instance.StopSound(EnumsManager.SoundEffect._BGM_GAMEPLAY_1);
            SoundManager.Instance.StopSound(EnumsManager.SoundEffect._BGM_CREDIT_PANEL);
            SoundManager.Instance.PlaySound(EnumsManager.SoundEffect._BGM_MAINMENU);
        }
        pauseMenuPanel.SetActive(false);
        levelTransitionPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        rewardPanel.SetActive(false);
        optionMenuPanel.SetActive(false);
        lastLevelPanel.SetActive(false);
        loadingPanel.SetActive(false);

        // Skip main menu after intro story scene
        if (PlayerPrefs.GetInt(PlayerPrefsKeys.IS_INTRO_STORY_CUTSCENE_ALREADY_PLAYED) == PlayerPrefsValues.TRUE &&
            PlayerPrefs.GetInt(PlayerPrefsKeys.IS_INTRO_STORY_CUTSCENE_TO_GAMEPLAY_CALLED) == PlayerPrefsValues.FALSE) {
            PlayGame();
            PlayerPrefs.SetInt(PlayerPrefsKeys.IS_INTRO_STORY_CUTSCENE_TO_GAMEPLAY_CALLED, PlayerPrefsValues.TRUE);
        }
    }

    void Update() {
        PauseInput();
        InventoryPanel();

        if (loadingPanel.activeSelf) {
            Vector3 mainCameraPos = Camera.main.transform.position;
            mainCameraPos.z = 0f;
            loadingPanel.transform.position = mainCameraPos;
        }
    }

    public void PlayGame() {
        Time.timeScale = 1f;
        SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.BUTTON_CLICK);
        loadingPanel.SetActive(true);
        loadingPanel.GetComponent<Animator>().Play(AnimationTags.LOADING);
        inventoryPanel.GetComponent<Animator>().Play(AnimationTags.INVENTORY_CLOSE);
        mainMenuPanel.SetActive(false);
        Invoke("StartGame", 2f);
    }

    void StartGame() {
        if (PlayerPrefs.GetInt(PlayerPrefsKeys.IS_INTRO_STORY_CUTSCENE_ALREADY_PLAYED) == PlayerPrefsValues.TRUE) {
            GameManager.Instance.ResetGameplay(EnumsManager.LevelState.LEVEL_1);
            GameManager.Instance.SetGameState(EnumsManager.GameState.GAMEPLAY);
            GameManager.Instance.TurnOnUILights(true);
            SoundManager.Instance.StopSound(EnumsManager.SoundEffect._BGM_MAINMENU);
            SoundManager.Instance.PlaySound(EnumsManager.SoundEffect._AMBIENCE_FOREST);
            inventoryPanel.GetComponent<Animator>().Play(AnimationTags.INVENTORY_OPEN);
        } else {
            SceneManager.LoadScene(SceneNames.INTRO_STORY);
        }
    }

    void InventoryPanel() {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.F)) {
            OpenInventory();
        }
    }

    void PauseInput() {
        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.IsGameStateGameplay()) {
            Pause();
        } else if (Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.IsGameStatePause() && !optionMenuPanel.activeSelf) {
            Resume();
        }
    }

    public void OpenInventory() {
        SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.BUTTON_CLICK);
        if (isInventoryOpen) {
            inventoryPanel.GetComponent<Animator>().Play(AnimationTags.INVENTORY_CLOSE);
            isInventoryOpen = false;
        } else {
            inventoryPanel.GetComponent<Animator>().Play(AnimationTags.INVENTORY_OPEN);
            isInventoryOpen = true;
        }
    }

    public void Pause() {
        SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.BUTTON_CLICK);
        GameManager.Instance.SetGameState(EnumsManager.GameState.PAUSE);
        gameplayPanel.SetActive(false);
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume() {
        SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.BUTTON_CLICK);
        GameManager.Instance.SetGameState(EnumsManager.GameState.GAMEPLAY);
        gameplayPanel.SetActive(true);
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void LevelTransition() {
        SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.BUTTON_CLICK);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tags.ENEMY);
        foreach (var enemy in enemies) {
            if (!enemy.GetComponent<Enemy>().IsEnemyTypeBoss()) {
                Destroy(enemy);
            }
        }
        Time.timeScale = 0f;
        GameManager.Instance.SetGameState(EnumsManager.GameState.LEVELTRANSITION);
        SkillEnhancer.Instance.InstantiateRandomSkill();
        levelTransitionPanel.SetActive(true);
    }

    public void GameOver() {
        SoundManager.Instance.StopSound(EnumsManager.SoundEffect.PLAYER_DYING);
        SoundManager.Instance.StopSound(EnumsManager.SoundEffect._BGM_GAMEPLAY_1);
        SoundManager.Instance.PlaySound(EnumsManager.SoundEffect._BGM_GAME_OVER);
        Time.timeScale = 0f;
        GameManager.Instance.SetGameState(EnumsManager.GameState.GAMEOVER);
        PostProcessingEffect.Instance.ResetDyingEffect();
        gameplayPanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    public void Reward() {
        HighScoreSystem.Instance.SetHighScore(GameManager.Instance.GetCurrentLevelState());
        Time.timeScale = 0f;
        GameManager.Instance.SetGameState(EnumsManager.GameState.REWARD);
        GameManager.Instance.SetGemsRewardFromCoinAndHealth();
        PostProcessingEffect.Instance.ResetDyingEffect();
        pauseMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        levelTransitionPanel.SetActive(false);
        lastLevelPanel.SetActive(false);
        rewardPanel.SetActive(true);
        SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.REWARD_PANEL);
    }

    public void ClaimReward() {
        GemsSystem.Instance.AddGems(
            GameManager.Instance.gemsObtainedFromLevel + 
            GameManager.Instance.gemsObtainedFromLeftoverCoin + 
            GameManager.Instance.gemsObtainedFromLeftoverHealth);
        SoundManager.Instance.StopSound(EnumsManager.SoundEffect._BGM_GAME_OVER);
        SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.BUTTON_CLICK);
        GameManager.Instance.SetGameState(EnumsManager.GameState.MAINMENU);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Option() {
        SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.BUTTON_CLICK);
        optionMenuPanel.SetActive(true);
        if (GameManager.Instance.IsGameStateMainMenu()) {
            gameplayPanel.SetActive(false);
            mainMenuPanel.SetActive(false);
        } else if (GameManager.Instance.IsGameStatePause()) {
            pauseMenuPanel.SetActive(false);
            gameplayPanel.SetActive(false);
        }
    }

    public void Back() {
        SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.BUTTON_CLICK);
        optionMenuPanel.SetActive(false);
        if (GameManager.Instance.IsGameStateMainMenu()) {
            gameplayPanel.SetActive(true);
            mainMenuPanel.SetActive(true);
        } else if (GameManager.Instance.IsGameStatePause()) {
            gameplayPanel.SetActive(true);
            pauseMenuPanel.SetActive(true);
        }
    }

    public void Quit() {
        SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.BUTTON_CLICK);
        Application.Quit();
    }

    public void NextLevel() {
        SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.PLAYER_POWER_UP);
        Time.timeScale = 1f;

        if (LevelManager.Instance.IsLastLevel()) {
            LastLevel();
        } else {
            GameManager.Instance.NextLevel();
        }
        
        levelTransitionPanel.SetActive(false);
        GameObject.FindGameObjectWithTag(Tags.PLAYER).GetComponent<Player>().isPowerUp = true;
    }

    void LastLevel() {
        SoundManager.Instance.StopSound(EnumsManager.SoundEffect._BGM_GAMEPLAY_1);
        SoundManager.Instance.PlaySound(EnumsManager.SoundEffect._BGM_CREDIT_PANEL);
        Time.timeScale = 0f;

        levelTransitionPanel.SetActive(false);
        lastLevelPanel.SetActive(true);
    }

    public void DeleteAllData() {
        SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.BUTTON_CLICK);
        GemsSystem.Instance.SubstractGems(GemsSystem.Instance.currentGems);
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    public void PlayClickSFX() => SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.BUTTON_CLICK);
}
