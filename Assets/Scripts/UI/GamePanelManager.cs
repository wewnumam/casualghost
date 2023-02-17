using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePanelManager : MonoBehaviour {
    public static GamePanelManager Instance { get; private set; }

    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject pauseMenuPanel;
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
        if (mainMenuPanel.activeInHierarchy) {
            gameState = GameState.MAINMENU;
            Time.timeScale = 0f;
        }
        gameOverPanel.SetActive(false);
    }

    public GameState GetGameState() => gameState;

    public void PlayGame() {
        gameState = GameState.GAMEPLAY;
        mainMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void GameOver() {
        gameState = GameState.GAMEOVER;
        gameOverPanel.SetActive(true);
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
    GAMEOVER,
    REWARD
}
