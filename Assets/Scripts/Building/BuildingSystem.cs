using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingSystem : MonoBehaviour {
    [SerializeField] private BuildingType buildingType;
    [SerializeField] private int unlockCost;
    [SerializeField] private TextMeshProUGUI costInfoText;
    private string intialCostInfoText;

    void Awake() {
        intialCostInfoText = costInfoText.text;    
    }

    bool CanUnlock() => PlayerPrefs.GetInt(PlayerPrefsKeys.GEMS) >= unlockCost;

    void Update() {
        if (buildingType == BuildingType.THORN_MINE && PlayerPrefs.GetInt(PlayerPrefsKeys.IS_THORNMINE_UNLOCKED) == PlayerPrefsValues.TRUE) {
            costInfoText.text = "";
        } else if (buildingType == BuildingType.DECOY && PlayerPrefs.GetInt(PlayerPrefsKeys.IS_DECOY_UNLOCKED) == PlayerPrefsValues.TRUE) {
            costInfoText.text = "";
        } else if (buildingType == BuildingType.CANON && PlayerPrefs.GetInt(PlayerPrefsKeys.IS_CANON_UNLOCKED) == PlayerPrefsValues.TRUE) {
            costInfoText.text = "";
        } else {
            costInfoText.text = intialCostInfoText;
        }
    }

    public void Unlock() {
        if (!CanUnlock()) return;

        PlayerPrefs.SetInt(PlayerPrefsKeys.GEMS, PlayerPrefs.GetInt(PlayerPrefsKeys.GEMS) - unlockCost);
        costInfoText.text = "";

        if (buildingType == BuildingType.THORN_MINE) {
            PlayerPrefs.SetInt(PlayerPrefsKeys.IS_THORNMINE_UNLOCKED, 1);
        } else if (buildingType == BuildingType.DECOY) {
            PlayerPrefs.SetInt(PlayerPrefsKeys.IS_DECOY_UNLOCKED, 1);
        } else if (buildingType == BuildingType.CANON) {
            PlayerPrefs.SetInt(PlayerPrefsKeys.IS_CANON_UNLOCKED, 1);
        }
    }
}

public enum BuildingType {
    ROOT,
    CLEAR_BUILDING,
    THORN_MINE,
    DECOY,
    CANON
}