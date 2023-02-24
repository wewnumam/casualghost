using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StorySceneController : MonoBehaviour {
    public StoryScene currentScene;
    public BottomBarController bottomBar;
    public BackgroundController backgroundController;

    void Awake() => Time.timeScale = 1f;

    void Start() {
        bottomBar.PlayScene(currentScene);
        backgroundController.SetImage(currentScene.background);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
            if (bottomBar.IsCompleted()) {
                if (bottomBar.IsLastSentence()) {
                    if (currentScene.isLastScene) {
                        PlayerPrefs.SetInt(PlayerPrefsKeys.IS_INTRO_STORY_CUTSCENE_ALREADY_PLAYED, PlayerPrefsValues.TRUE);
                        SceneManager.LoadScene(SceneNames.GAMEPLAY);
                    }
                    currentScene = currentScene.nextScene;
                    bottomBar.PlayScene(currentScene);
                    backgroundController.SwitchImage(currentScene.background);
                } else {
                    bottomBar.PlayNextSentence();
                }
            }
        }
    }
}
