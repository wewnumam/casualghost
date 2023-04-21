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
    private int missionProgressMaxValue;
    private int missionProgressValue;

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

        foreach (var mission in missions) {
            GameObject missionList = Instantiate(listTemplate, parent);
            missionList.GetComponentsInChildren<TextMeshProUGUI>()[0].text = mission.missionTitle;
            missionList.GetComponentInChildren<Slider>().maxValue = mission.maxValue;
            missionList.GetComponentInChildren<Slider>().value = mission.value;
            missionList.GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"{mission.value}/{mission.maxValue}";
            mission.missionList = missionList;
            missionProgressMaxValue += (int)mission.maxValue;
        }
        missionProgress.maxValue = missionProgressMaxValue;
    }

    public void UpdateMissionProgress(EnumsManager.Mission missionTag, float value) {
        Mission mission = missions.Find(m => m.missionTag == missionTag);
        if (mission.missionList != null) {
            mission.value = value;
            mission.missionList.GetComponentInChildren<Slider>().value = value;
            mission.missionList.GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"{mission.value}/{mission.maxValue}";
        }

        missionProgressValue = 0;
        foreach (var m in missions) {
            missionProgressValue += (int)m.value;
        }
        missionProgress.value = missionProgressValue;
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
