using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponSystem : MonoBehaviour {
    [SerializeField] private EnumsManager.WeaponType weaponType;

    [Header("Economic Settings")]
    [SerializeField] private int unlockCost;

    [Header("UI Properties")]
    [SerializeField] private TextMeshProUGUI costInfoText;
    private string intialCostInfoText;

    void Awake() {
        intialCostInfoText = costInfoText.text;
    }

    void Update() {
        SetCostInfoText();
        ModifyImageColorAlpha();
    }

    void SetCostInfoText() {
         // If the building type is THORN_MINE and it is already unlocked, then hide the cost information text
        switch (weaponType) {
            case EnumsManager.WeaponType.SHOTGUN when IsWeaponUnlocked(PlayerPrefsKeys.IS_SHOTGUN_UNLOCKED):
                costInfoText.text = "";
                break;
            default:
                // Otherwise, display the initial value of the cost information text
                costInfoText.text = intialCostInfoText;
                break;
        }
    }

    void ModifyImageColorAlpha() {
        PlayerShooting playerShooting = GameObject.FindGameObjectWithTag(Tags.PLAYER).GetComponentInChildren<PlayerShooting>();

        // Modify the alpha value of the image color of the building UI element based on whether the player can afford to build it.
        Color imageColor = GetComponent<Image>().color;
        if (playerShooting.currentWeaponType == weaponType) {
            imageColor.a = 0.5f;
        } else {
            imageColor.a = 1f;
        }
        GetComponent<Image>().color = imageColor;
    }

    // Helper method for checking if the building is already unlocked based on the player preferences key
    bool IsWeaponUnlocked(string playerPrefsKey) => PlayerPrefs.GetInt(playerPrefsKey) == PlayerPrefsValues.TRUE;

    // Helper method for checking if the player has enough gems to unlock the building
    bool CanUnlock() => PlayerPrefs.GetInt(PlayerPrefsKeys.GEMS) >= unlockCost;

    public void Unlock() {
        // If the player doesn't have enough gems to unlock the building, then return
        if (!CanUnlock()) return;

        if (!IsWeaponUnlocked(PlayerPrefsKeys.IS_SHOTGUN_UNLOCKED)) {
            SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.BUY_BUILDING);

            // Deduct the unlock cost from the player's gems
            PlayerPrefs.SetInt(PlayerPrefsKeys.GEMS, PlayerPrefs.GetInt(PlayerPrefsKeys.GEMS) - unlockCost);

            // Hide the cost information text in the UI
            costInfoText.text = "";
        }

        // Update the player preferences based on the building type
        if (weaponType == EnumsManager.WeaponType.SHOTGUN && !IsWeaponUnlocked(PlayerPrefsKeys.IS_SHOTGUN_UNLOCKED)) {
            PlayerPrefs.SetInt(PlayerPrefsKeys.IS_SHOTGUN_UNLOCKED, PlayerPrefsValues.TRUE);
        }
    }

    public void SwitchWeapon() {
        SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.BUTTON_CLICK);

        PlayerShooting playerShooting = GameObject.FindGameObjectWithTag(Tags.PLAYER).GetComponentInChildren<PlayerShooting>();
        
        if (weaponType == EnumsManager.WeaponType.DEFAULT) {
            PlayerPrefs.SetInt(PlayerPrefsKeys.WEAPON, PlayerPrefsValues.WEAPON_DEFAULT);
            playerShooting.WeaponSwitch(PlayerPrefsValues.WEAPON_DEFAULT);
        } else if (weaponType == EnumsManager.WeaponType.SHOTGUN && IsWeaponUnlocked(PlayerPrefsKeys.IS_SHOTGUN_UNLOCKED)) {
            PlayerPrefs.SetInt(PlayerPrefsKeys.WEAPON, PlayerPrefsValues.WEAPON_SHOTGUN);
            playerShooting.WeaponSwitch(PlayerPrefsValues.WEAPON_SHOTGUN);
        }
    }
}
