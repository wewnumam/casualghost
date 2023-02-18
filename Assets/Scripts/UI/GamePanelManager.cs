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
    

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Start () {
        mainMenuPanel.SetActive(true);
        if (mainMenuPanel.activeInHierarchy) {
            GameManager.Instance.SetGameState(GameState.MAINMENU);
            Time.timeScale = 0f;
        }
        inventoryPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
        levelTransitionPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        rewardPanel.SetActive(false);
    }

    void Update() {
        Pause();
        InventoryPanel();
    }

    public void PlayGame() {
        GameManager.Instance.SetGameState(GameState.GAMEPLAY);
        mainMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    void InventoryPanel() {
        if (Input.GetKeyDown(KeyCode.F)) {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }

    void Pause() {
        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.IsGameStateGameplay()) {
            GameManager.Instance.SetGameState(GameState.PAUSE);
            pauseMenuPanel.SetActive(true);
            Time.timeScale = 0f;
        } else if (Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.IsGameStatePause()) {
            Resume();
        }
    }

    public void Resume() {
        GameManager.Instance.SetGameState(GameState.GAMEPLAY);
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void LevelTransition() {
        Time.timeScale = 0f;
        GameManager.Instance.SetGameState(GameState.LEVELTRANSITION);
        levelTransitionPanel.SetActive(true);
    }

    public void GameOver() {
        GameManager.Instance.SetGameState(GameState.GAMEOVER);
        gameOverPanel.SetActive(true);
    }

    public void Reward() {
        Time.timeScale = 0f;
        GameManager.Instance.SetGameState(GameState.REWARD);
        gameOverPanel.SetActive(false);
        rewardPanel.SetActive(true);
    }

    public void MainMenu() {
        GameManager.Instance.SetGameState(GameState.MAINMENU);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
