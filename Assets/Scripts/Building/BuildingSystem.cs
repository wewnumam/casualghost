using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingSystem : MonoBehaviour {
    [SerializeField] private EnumsManager.BuildingType buildingType;

    [Header("Economic Settings")]
    [SerializeField] private int unlockCost;

    [Header("UI Properties")]
    [SerializeField] private TextMeshProUGUI costInfoText;
    private string intialCostInfoText;

    void Awake() {
        intialCostInfoText = costInfoText.text;    
    }

    void Update() {
        // Updates the cost information text in the UI
        SetCostInfoText();
    }

    // Helper method for updating the cost information text in the UI based on the building type and unlock status
    void SetCostInfoText() {
         // If the building type is THORN_MINE and it is already unlocked, then hide the cost information text
        switch (buildingType) {
            case EnumsManager.BuildingType.THORN_MINE when IsBuildingUnlocked(PlayerPrefsKeys.IS_THORNMINE_UNLOCKED):
                costInfoText.text = string.Empty;
                costInfoText.gameObject.SetActive(false);
                break;
            case EnumsManager.BuildingType.DECOY when IsBuildingUnlocked(PlayerPrefsKeys.IS_DECOY_UNLOCKED):
                costInfoText.text = string.Empty;
                costInfoText.gameObject.SetActive(false);
                break;
            case EnumsManager.BuildingType.CANON when IsBuildingUnlocked(PlayerPrefsKeys.IS_CANNON_UNLOCKED):
                costInfoText.text = string.Empty;
                costInfoText.gameObject.SetActive(false);
                break;
            default:
                // Otherwise, display the initial value of the cost information text
                costInfoText.text = intialCostInfoText;
                costInfoText.gameObject.SetActive(true);
                break;
        }
    }

    // Helper method for checking if the building is already unlocked based on the player preferences key
    bool IsBuildingUnlocked(string playerPrefsKey) => PlayerPrefs.GetInt(playerPrefsKey) == PlayerPrefsValues.TRUE;

    // Helper method for checking if the player has enough gems to unlock the building
    bool CanUnlock() => GemsSystem.Instance.currentGems >= unlockCost;

    public void Unlock() {
        // If the player doesn't have enough gems to unlock the building, then return
        if (!CanUnlock()) {
            SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.BUTTON_DISABLED);
            return;
        } 

        if (buildingType == EnumsManager.BuildingType.THORN_MINE && IsBuildingUnlocked(PlayerPrefsKeys.IS_THORNMINE_UNLOCKED)) return;
        if (buildingType == EnumsManager.BuildingType.DECOY && IsBuildingUnlocked(PlayerPrefsKeys.IS_DECOY_UNLOCKED)) return;
        if (buildingType == EnumsManager.BuildingType.CANON && IsBuildingUnlocked(PlayerPrefsKeys.IS_CANNON_UNLOCKED)) return;

        SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.BUY_BUILDING);

        // Deduct the unlock cost from the player's gems
        GemsSystem.Instance.SubstractGems(unlockCost);

        // Hide the cost information text in the UI
        costInfoText.text = string.Empty;
        costInfoText.gameObject.SetActive(false);

        // Update the player preferences based on the building type
        switch (buildingType) {
            case EnumsManager.BuildingType.THORN_MINE:
                PlayerPrefs.SetInt(PlayerPrefsKeys.IS_THORNMINE_UNLOCKED, PlayerPrefsValues.TRUE);
                break;
            case EnumsManager.BuildingType.DECOY:
                PlayerPrefs.SetInt(PlayerPrefsKeys.IS_DECOY_UNLOCKED, PlayerPrefsValues.TRUE);
                break;
            case EnumsManager.BuildingType.CANON:
                PlayerPrefs.SetInt(PlayerPrefsKeys.IS_CANNON_UNLOCKED, PlayerPrefsValues.TRUE);
                break;
        }

    }
}