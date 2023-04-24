using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {
    [Header("Tutorial Ilustration GameObject")]
    [SerializeField] private GameObject moveTutorial;
    [SerializeField] private GameObject shootTutorial;
    [SerializeField] private GameObject coinTutorial;
    [SerializeField] private GameObject buildTutorial;
    [SerializeField] private GameObject unlockTutorial;

    [Header("Completion Check Properties")]
    [SerializeField] private GameObject buildingParent;
    private bool isMoveDone, isSprintDone;
    private bool isShootDone, isReloadDone;
    private bool isCoinDone;
    private bool isBuildDone;
    private bool isUnlockDone;
    private int progressValue;

    void Start() {
        PlayerPrefsCheck();
    }

    void Update() {
        PlayerInput();
        TutorialCompletionCheck();
    }

    void PlayerPrefsCheck() {
        if (PlayerPrefs.GetInt(PlayerPrefsKeys.IS_INTRO_STORY_CUTSCENE_ALREADY_PLAYED) == PlayerPrefsValues.FALSE) {
            PlayerPrefs.SetInt(PlayerPrefsKeys.IS_TUTORIAL_MOVE_DONE, PlayerPrefsValues.FALSE);
            PlayerPrefs.SetInt(PlayerPrefsKeys.IS_TUTORIAL_SHOOT_DONE, PlayerPrefsValues.FALSE);
            PlayerPrefs.SetInt(PlayerPrefsKeys.IS_TUTORIAL_COIN_DONE, PlayerPrefsValues.FALSE);
            PlayerPrefs.SetInt(PlayerPrefsKeys.IS_TUTORIAL_BUILD_DONE, PlayerPrefsValues.FALSE);
            PlayerPrefs.SetInt(PlayerPrefsKeys.IS_TUTORIAL_UNLOCK_DONE, PlayerPrefsValues.FALSE);
        }

        if (PlayerPrefs.GetInt(PlayerPrefsKeys.IS_TUTORIAL_MOVE_DONE) == PlayerPrefsValues.TRUE) {
            isMoveDone = true;
            isSprintDone = true;
        }

        if (PlayerPrefs.GetInt(PlayerPrefsKeys.IS_TUTORIAL_SHOOT_DONE) == PlayerPrefsValues.TRUE) {
            isShootDone = true;
            isReloadDone = true;
        }

        if (PlayerPrefs.GetInt(PlayerPrefsKeys.IS_TUTORIAL_COIN_DONE) == PlayerPrefsValues.TRUE) {
            isCoinDone = true;
        }

        if (PlayerPrefs.GetInt(PlayerPrefsKeys.IS_TUTORIAL_BUILD_DONE) == PlayerPrefsValues.TRUE) {
            isBuildDone = true;
        }

        if (PlayerPrefs.GetInt(PlayerPrefsKeys.IS_TUTORIAL_UNLOCK_DONE) == PlayerPrefsValues.TRUE) {
            isUnlockDone = true;
            unlockTutorial.SetActive(false);
        }

        if (isMoveDone && isSprintDone && isShootDone && isReloadDone && isCoinDone && isBuildDone && isUnlockDone) {
            Destroy(gameObject);
        }
    }

    void UpdateMissionProgress() {
        MissionManager.Instance.UpdateMissionProgress(EnumsManager.Mission.TUTORIAL_COMPLETTION);
    }

    void PlayerInput() {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)) {
            isMoveDone = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            isSprintDone = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.R)) {
            isReloadDone = true;
        }
    }

    void TutorialCompletionCheck() {
        // Move Tutorial
        if (isMoveDone && isSprintDone) {
            PlayerPrefs.SetInt(PlayerPrefsKeys.IS_TUTORIAL_MOVE_DONE, PlayerPrefsValues.TRUE);
            if (moveTutorial.activeSelf && GameManager.Instance.IsGameStateGameplay()) {
                SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.TUTORIAL_DONE);
                UpdateMissionProgress();
            }
            moveTutorial.SetActive(false);
        }

        // Shoot Tutorial
        if ((GameObject.FindGameObjectsWithTag(Tags.BULLET_TYPE_ONE).Length > 1 || isShootDone) && isReloadDone) {
            PlayerPrefs.SetInt(PlayerPrefsKeys.IS_TUTORIAL_SHOOT_DONE, PlayerPrefsValues.TRUE);
            if (shootTutorial.activeSelf && GameManager.Instance.IsGameStateGameplay()) {
                SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.TUTORIAL_DONE);
                UpdateMissionProgress();
            }
            shootTutorial.SetActive(false);
        }

        // Coin Tutorial
        if (CoinSystem.Instance.GetCurrentCoin() > 0 || isCoinDone) {
            PlayerPrefs.SetInt(PlayerPrefsKeys.IS_TUTORIAL_COIN_DONE, PlayerPrefsValues.TRUE);
            if (coinTutorial.activeSelf && GameManager.Instance.IsGameStateGameplay()) {
                SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.TUTORIAL_DONE);
                UpdateMissionProgress();
            }
            coinTutorial.SetActive(false);
        }

        // Build Tutorial
        if (buildingParent.transform.childCount > 1 || isBuildDone) {
            PlayerPrefs.SetInt(PlayerPrefsKeys.IS_TUTORIAL_BUILD_DONE, PlayerPrefsValues.TRUE);
            if (buildTutorial.activeSelf && GameManager.Instance.IsGameStateGameplay()) {
                SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.TUTORIAL_DONE);
                UpdateMissionProgress();
            }
            buildTutorial.SetActive(false);
        }

        // Unlock Tutorial
        if (PlayerPrefs.GetInt(PlayerPrefsKeys.IS_THORNMINE_UNLOCKED) == PlayerPrefsValues.TRUE ||
            PlayerPrefs.GetInt(PlayerPrefsKeys.IS_DECOY_UNLOCKED) == PlayerPrefsValues.TRUE ||
            PlayerPrefs.GetInt(PlayerPrefsKeys.IS_CANNON_UNLOCKED) == PlayerPrefsValues.TRUE ||
            isUnlockDone) {
            PlayerPrefs.SetInt(PlayerPrefsKeys.IS_TUTORIAL_UNLOCK_DONE, PlayerPrefsValues.TRUE);
            if (unlockTutorial.activeSelf) {
                SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.TUTORIAL_DONE);
                UpdateMissionProgress();
                unlockTutorial.SetActive(false);
            }
        }
    }
}
