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
        SoundManager.Instance.StopSound(EnumsManager.SoundEffect._BGM_MAINMENU);
        SoundManager.Instance.PlaySound(EnumsManager.SoundEffect._BGM_INTRO_STORY_MAGICAL);
        bottomBar.PlayScene(currentScene);
        backgroundController.SetImage(currentScene.background);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
            if (!bottomBar.IsCompleted()) return;
            
            if (bottomBar.IsLastSentence()) {
                if (currentScene.isLastScene) {
                    LoadGameplay();
                } else {
                    currentScene = currentScene.nextScene;
                    bottomBar.PlayScene(currentScene);
                    backgroundController.SwitchImage(currentScene.background);
                    if (currentScene.name == "Scene8") {
                        SoundManager.Instance.StopSound(EnumsManager.SoundEffect._BGM_INTRO_STORY_MAGICAL);
                        SoundManager.Instance.PlaySound(EnumsManager.SoundEffect._BGM_INTRO_STORY_TENSE);
                    }
                }
            } else {
                bottomBar.PlayNextSentence();
            }
        } else if (Input.GetKeyDown(KeyCode.Escape)) {
            LoadGameplay();
        }
    }

    void LoadGameplay() {
        SoundManager.Instance.StopSound(EnumsManager.SoundEffect._BGM_INTRO_STORY_MAGICAL);
        SoundManager.Instance.StopSound(EnumsManager.SoundEffect._BGM_INTRO_STORY_TENSE);
        PlayerPrefs.SetInt(PlayerPrefsKeys.IS_INTRO_STORY_CUTSCENE_ALREADY_PLAYED, PlayerPrefsValues.TRUE);
        SceneManager.LoadScene(SceneNames.GAMEPLAY);
    }
}
