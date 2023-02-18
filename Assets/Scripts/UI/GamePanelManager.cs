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
    private GameState gameState;

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
            gameState = GameState.MAINMENU;
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

    public GameState GetGameState() => gameState;

    public void PlayGame() {
        gameState = GameState.GAMEPLAY;
        mainMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    void InventoryPanel() {
        if (Input.GetKeyDown(KeyCode.F)) {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }

    void Pause() {
        if (Input.GetKeyDown(KeyCode.Escape) && gameState == GameState.GAMEPLAY) {
            gameState = GameState.PAUSE;
            pauseMenuPanel.SetActive(true);
            Time.timeScale = 0f;
        } else if (Input.GetKeyDown(KeyCode.Escape) && gameState == GameState.PAUSE) {
            Resume();
        }
    }

    public void Resume() {
        gameState = GameState.GAMEPLAY;
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void LevelTransition() {
        Time.timeScale = 0f;
        gameState = GameState.LEVELTRANSITION;
        levelTransitionPanel.SetActive(true);
    }

    public void GameOver() {
        gameState = GameState.GAMEOVER;
        gameOverPanel.SetActive(true);
    }

    public void Reward() {
        Time.timeScale = 0f;
        gameState = GameState.REWARD;
        gameOverPanel.SetActive(false);
        rewardPanel.SetActive(true);
    }

    public void MainMenu() {
        gameState = GameState.MAINMENU;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
