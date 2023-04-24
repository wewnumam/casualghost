using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GemsSystem : MonoBehaviour {
    public static GemsSystem Instance { get; private set; }

    [Header("Gems Settings")]
    private int _currentGems;
    public int currentGems { get => _currentGems; }

    [Header("UI Properties")]
    [SerializeField] private TextMeshProUGUI gemsInfoText;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        _currentGems = PlayerPrefs.GetInt(PlayerPrefsKeys.GEMS);
    }

    public void AddGems(int amount) {
        _currentGems += amount;
        PlayerPrefs.SetInt(PlayerPrefsKeys.GEMS, _currentGems);

        MissionManager.Instance.UpdateMissionProgress(EnumsManager.Mission.NUMBER_OF_GEMS_CLAIMED, amount);
    }

    public void SubstractGems(int amount) {
        _currentGems -= amount;
        PlayerPrefs.SetInt(PlayerPrefsKeys.GEMS, _currentGems);
    }

    void Update() {
        SetGemsInfo();
    }

    void SetGemsInfo() {
        gemsInfoText.text = PlayerPrefs.GetInt(PlayerPrefsKeys.GEMS).ToString();
	}
}
