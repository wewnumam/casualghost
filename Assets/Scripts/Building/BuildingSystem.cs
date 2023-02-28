using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingSystem : MonoBehaviour {
    [SerializeField] private EnumsManager.BuildingType buildingType;
    [SerializeField] private int unlockCost;
    [SerializeField] private TextMeshProUGUI costInfoText;
    private string intialCostInfoText;

    void Awake() {
        intialCostInfoText = costInfoText.text;    
    }

    void Update() {
        SetCostInfoText();
    }

    void SetCostInfoText() {
        switch (buildingType) {
            case EnumsManager.BuildingType.THORN_MINE when IsBuildingUnlocked(PlayerPrefsKeys.IS_THORNMINE_UNLOCKED):
                costInfoText.text = "";
                break;
            case EnumsManager.BuildingType.DECOY when IsBuildingUnlocked(PlayerPrefsKeys.IS_DECOY_UNLOCKED):
                costInfoText.text = "";
                break;
            case EnumsManager.BuildingType.CANON when IsBuildingUnlocked(PlayerPrefsKeys.IS_CANON_UNLOCKED):
                costInfoText.text = "";
                break;
            default:
                costInfoText.text = intialCostInfoText;
                break;
        }
    }

    bool IsBuildingUnlocked(string playerPrefsKey) => PlayerPrefs.GetInt(playerPrefsKey) == PlayerPrefsValues.TRUE;
    bool CanUnlock() => PlayerPrefs.GetInt(PlayerPrefsKeys.GEMS) >= unlockCost;

    public void Unlock() {
        if (!CanUnlock()) return;

        PlayerPrefs.SetInt(PlayerPrefsKeys.GEMS, PlayerPrefs.GetInt(PlayerPrefsKeys.GEMS) - unlockCost);
        costInfoText.text = "";

        if (buildingType == EnumsManager.BuildingType.THORN_MINE) {
            PlayerPrefs.SetInt(PlayerPrefsKeys.IS_THORNMINE_UNLOCKED, PlayerPrefsValues.TRUE);
        } else if (buildingType == EnumsManager.BuildingType.DECOY) {
            PlayerPrefs.SetInt(PlayerPrefsKeys.IS_DECOY_UNLOCKED, PlayerPrefsValues.TRUE);
        } else if (buildingType == EnumsManager.BuildingType.CANON) {
            PlayerPrefs.SetInt(PlayerPrefsKeys.IS_CANON_UNLOCKED, PlayerPrefsValues.TRUE);
        }
    }
}