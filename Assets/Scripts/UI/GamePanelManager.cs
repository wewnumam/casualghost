using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePanelManager : MonoBehaviour {
    public static GamePanelManager Instance { get; private set; }

    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject levelTransitionPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject rewardPanel;
    [SerializeField] private GameObject optionMenuPanel;
    [SerializeField] private GameObject lastLevelPanel;

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
            Time.timeScale = 0f;
        }
        pauseMenuPanel.SetActive(false);
        levelTransitionPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        rewardPanel.SetActive(false);
        optionMenuPanel.SetActive(false);
        lastLevelPanel.SetActive(false);
    }

    void Update() {
        PauseInput();
        InventoryPanel();
    }

    public void PlayGame() {
        if (PlayerPrefs.GetInt(PlayerPrefsKeys.IS_INTRO_STORY_CUTSCENE_ALREADY_PLAYED) == PlayerPrefsValues.TRUE) {
            GameManager.Instance.SetGameState(EnumsManager.GameState.GAMEPLAY);
            mainMenuPanel.SetActive(false);
            Time.timeScale = 1f;
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
        } else if (Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.IsGameStatePause()) {
            Resume();
        }
    }

    public void OpenInventory() {
        if (isInventoryOpen) {
            inventoryPanel.GetComponent<Animator>().Play(AnimationTags.INVENTORY_CLOSE);
            isInventoryOpen = false;
        } else {
            inventoryPanel.GetComponent<Animator>().Play(AnimationTags.INVENTORY_OPEN);
            isInventoryOpen = true;
        }
    }

    public void Pause() {
        GameManager.Instance.SetGameState(EnumsManager.GameState.PAUSE);
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume() {
        GameManager.Instance.SetGameState(EnumsManager.GameState.GAMEPLAY);
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void LevelTransition() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tags.ENEMY);
        foreach (var enemy in enemies) {
            Destroy(enemy);
        }
        Time.timeScale = 0f;
        GameManager.Instance.SetGameState(EnumsManager.GameState.LEVELTRANSITION);
        SkillEnhancer.Instance.InstantiateRandomSkill();
        levelTransitionPanel.SetActive(true);
    }

    public void GameOver() {
        GameManager.Instance.SetGameState(EnumsManager.GameState.GAMEOVER);
        gameOverPanel.SetActive(true);
    }

    public void Reward() {
        Time.timeScale = 0f;
        GameManager.Instance.SetGameState(EnumsManager.GameState.REWARD);
        GameManager.Instance.SetGemsRewardFromCoinAndHealth();
        pauseMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        levelTransitionPanel.SetActive(false);
        lastLevelPanel.SetActive(false);
        rewardPanel.SetActive(true);
    }

    public void ClaimReward() {
        GameManager.Instance.ClaimReward();        
        GameManager.Instance.SetGameState(EnumsManager.GameState.MAINMENU);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Option() {
        optionMenuPanel.SetActive(true);
        if (GameManager.Instance.IsGameStateMainMenu()) {
            mainMenuPanel.SetActive(false);
        } else if (GameManager.Instance.IsGameStatePause()) {
            pauseMenuPanel.SetActive(false);
        }
    }

    public void Back() {
        optionMenuPanel.SetActive(false);
        if (GameManager.Instance.IsGameStateMainMenu()) {
            mainMenuPanel.SetActive(true);
        } else if (GameManager.Instance.IsGameStatePause()) {
            pauseMenuPanel.SetActive(true);
        }
    }

    public void Quit() {
        Application.Quit();
    }

    public void NextLevel() {
        Time.timeScale = 1f;

        if (LevelManager.Instance.IsLastLevel()) {
            LastLevel();
        } else {
            GameManager.Instance.NextLevel();
        }
        
        levelTransitionPanel.SetActive(false);
    }

    void LastLevel() {
        Time.timeScale = 0f;

        levelTransitionPanel.SetActive(false);
        lastLevelPanel.SetActive(true);
    }

    public void DeleteAllData() {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

}
