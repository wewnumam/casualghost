using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreSystem : MonoBehaviour {
    public static HighScoreSystem Instance { get; private set; }
    private EnumsManager.LevelState _currentHighScore;
    public EnumsManager.LevelState currentHighScore { get => _currentHighScore; }

    void Awake () {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        _currentHighScore = (EnumsManager.LevelState)PlayerPrefs.GetInt(PlayerPrefsKeys.HIGH_SCORE);
    }

    public void SetHighScore(EnumsManager.LevelState currentLevelState) {
        if (currentLevelState >= _currentHighScore) {
            _currentHighScore = currentLevelState;
            PlayerPrefs.SetInt(PlayerPrefsKeys.HIGH_SCORE, (int)_currentHighScore);
        }
    }
}