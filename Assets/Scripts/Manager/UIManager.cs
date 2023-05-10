using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using TMPro;

public class UIManager : MonoBehaviour {
    public static UIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TooltipTrigger timerTooltip;
    [SerializeField] private TextMeshProUGUI[] levelInfoText;
    [SerializeField] private TooltipTrigger levelTooltip;
    [SerializeField] private TextMeshProUGUI gemsInfoText;
    [SerializeField] private TextMeshProUGUI rewardInfoText;
    [SerializeField] private Slider levelProgress;
    [SerializeField] private Slider timeProgress;
    [SerializeField] private TextMeshProUGUI countdownText;
    private StringBuilder timerTextBuilder = new StringBuilder();
    private StringBuilder levelInfoTextBuilder = new StringBuilder();
    private string currentLevelText;
    [SerializeField] private Light2D[] UILights;
    private float[] UILightsIntensity;
    [SerializeField] private GameObject[] huds;

    void Awake () {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        UILightsIntensity = new float[UILights.Length];
        for (int i = 0; i < UILights.Length; i++) {
            UILightsIntensity[i] = UILights[i].intensity;
        }
    }

    public IEnumerator Countdown() {
        countdownText.gameObject.SetActive(true);
        countdownText.text = "READY";
        yield return new WaitForSeconds(2f);
        countdownText.text = "3";
        yield return new WaitForSeconds(1f);
        countdownText.text = "2";
        yield return new WaitForSeconds(1f);
        countdownText.text = "1";
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);
    }

    public void SetTimerInfo() {
        TimeSpan time = TimeSpan.FromSeconds(GameManager.Instance.currentTime);
        timerTextBuilder.Length = 0;
        timerTextBuilder.Append($"{time:mm\\:ss}");
        timerText.text = timerTextBuilder.ToString();
        timeProgress.maxValue = GameManager.Instance.playTimeInSeconds;
        timeProgress.value = GameManager.Instance.currentTime;
        timerTooltip.header = timerTextBuilder.ToString();
    }

    public void SetLevelInfo() {
        currentLevelText = Enum.GetName(typeof(EnumsManager.LevelState), GameManager.Instance.GetCurrentLevelState());

        levelInfoTextBuilder.Length = 0;
        levelInfoTextBuilder.Append(currentLevelText);
        levelInfoTextBuilder.Append(" DONE!");
        levelInfoText[0].text = levelInfoTextBuilder.ToString();

        levelInfoTextBuilder.Length = 0;
        levelInfoTextBuilder.Append(currentLevelText);
        levelInfoText[1].text = levelInfoTextBuilder.ToString();

        levelInfoTextBuilder.Length = 0;
        levelInfoTextBuilder.Append("Current Level: ");
        levelInfoTextBuilder.Append(currentLevelText);
        levelInfoText[2].text = levelInfoTextBuilder.ToString();

        levelInfoTextBuilder.Length = 0;
        levelInfoTextBuilder.Append("Longest Level: ");
        if (GameManager.Instance.GetCurrentLevelState() > HighScoreSystem.Instance.currentHighScore) {
            levelInfoTextBuilder.Append(currentLevelText);
            levelInfoText[3].text = levelInfoTextBuilder.ToString();
            levelInfoText[4].gameObject.SetActive(true);
        } else {
            levelInfoTextBuilder.Append(Enum.GetName(typeof(EnumsManager.LevelState), HighScoreSystem.Instance.currentHighScore));
            levelInfoText[3].text = levelInfoTextBuilder.ToString();
            levelInfoText[4].gameObject.SetActive(false);
        }

        levelProgress.maxValue = LevelManager.Instance.levelStates.Length;
        levelProgress.value = (int)GameManager.Instance.GetCurrentLevelState() + 1;

        levelInfoTextBuilder.Length = 0;
        levelInfoTextBuilder.Append(currentLevelText);
        levelTooltip.header = levelInfoTextBuilder.ToString();
    }

    public void SetRewardInfo() {
        rewardInfoText.text = $"Level:    {GameManager.Instance.gemsObtainedFromLevel,3}\nLeftover Coin:    {GameManager.Instance.gemsObtainedFromLeftoverCoin,3}\nLeftover Health:    {GameManager.Instance.gemsObtainedFromLeftoverHealth,3}\n";
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

    public void HideHuds() {
        for (int i = 0; i < huds.Length; i++) {
            if (huds[i].activeSelf) {
                huds[i].SetActive(false);
            } else {
                huds[i].SetActive(true);
            }
        }
    }
}
