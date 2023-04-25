using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionManager : MonoBehaviour {
    public static MissionManager Instance { get; private set; }
    
    [SerializeField] private Transform parent;
    [SerializeField] private GameObject listTemplate;
    public List<Mission> missions;
    [SerializeField] private Slider missionProgress;
    [SerializeField] private TextMeshProUGUI missionProgressText;
    private int missionProgressMaxValue;
    private int _missionProgressValue;
    public int missionProgressValue { get => _missionProgressValue; }

    void Awake () {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        foreach (Transform child in parent) {
            Destroy(child.gameObject);
        }

        missionProgressMaxValue = 100;
        _missionProgressValue = 0;
        foreach (var mission in missions) {
            mission.value = PlayerPrefs.GetInt(GetPlayerPrefsKeyByTag(mission.missionTag));
            _missionProgressValue += (int)((missionProgressMaxValue / missions.Count) * (mission.value / mission.maxValue));

            GameObject missionList = Instantiate(listTemplate, parent);
            missionList.GetComponentsInChildren<TextMeshProUGUI>()[0].text = mission.missionTitle;
            missionList.GetComponentInChildren<Slider>().maxValue = mission.maxValue;
            missionList.GetComponentInChildren<Slider>().value = mission.value;
            missionList.GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"{mission.value}/{mission.maxValue}";
            mission.missionList = missionList;
        }
        missionProgress.maxValue = missionProgressMaxValue;
        missionProgress.value = _missionProgressValue;
        missionProgressText.text = $"{_missionProgressValue}%";
    }

    public void UpdateMissionProgress(EnumsManager.Mission missionTag, int addBy = 1) {
        int valueCounter = PlayerPrefs.GetInt(GetPlayerPrefsKeyByTag(missionTag));
        valueCounter += addBy;
        PlayerPrefs.SetInt(GetPlayerPrefsKeyByTag(missionTag), valueCounter);

        Mission mission = missions.Find(m => m.missionTag == missionTag);
        if (mission.missionList != null) {
            mission.value = valueCounter;
            mission.missionList.GetComponentInChildren<Slider>().value = valueCounter;
            mission.missionList.GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"{mission.value}/{mission.maxValue}";
        }

        _missionProgressValue = 0;
        foreach (var m in missions) {
            _missionProgressValue += (int)((missionProgressMaxValue / missions.Count) * (m.value / m.maxValue));
        }
        missionProgress.value = _missionProgressValue;
        missionProgressText.text = $"{_missionProgressValue}%";
    }

    string GetPlayerPrefsKeyByTag(EnumsManager.Mission missionTag) {
        string playerPrefsKey = "";
        if (missionTag ==  EnumsManager.Mission.TUTORIAL_COMPLETTION) {
            playerPrefsKey = PlayerPrefsKeys.TUTORIAL_COMPLETED_COUNTER;
        } else if (missionTag ==  EnumsManager.Mission.NUMBER_OF_ENEMIES_KILLED) {
            playerPrefsKey = PlayerPrefsKeys.ENEMY_KILLED_COUNTER;
        } else if (missionTag ==  EnumsManager.Mission.NUMBER_OF_GAME_WINS) {
            playerPrefsKey = PlayerPrefsKeys.WIN_COUNTER;
        } else if (missionTag ==  EnumsManager.Mission.NUMBER_OF_GEMS_CLAIMED) {
            playerPrefsKey = PlayerPrefsKeys.GEMS_CLAIMED_COUNTER;
        } else if (missionTag ==  EnumsManager.Mission.NUMBER_OF_LEVELS_PLAYED) {
            playerPrefsKey = PlayerPrefsKeys.LEVEL_PLAYED_COUNTER;
        }
        return playerPrefsKey;
}
}

[System.Serializable]
public class Mission {
    public EnumsManager.Mission missionTag;
    public GameObject missionList;
    public string missionTitle;
    public float value;
    public float maxValue;

    public void SetValue(float value) => this.value = value;
}
