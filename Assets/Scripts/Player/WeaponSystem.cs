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
    [SerializeField] private Sprite initialImage;
    [SerializeField] private Sprite highlightedImage;

    void Awake() {
        intialCostInfoText = costInfoText.text;
    }

    void Update() {
        SetCostInfoText();
        ModifyImage();
    }

    void SetCostInfoText() {
         // If the building type is THORN_MINE and it is already unlocked, then hide the cost information text
        switch (weaponType) {
            case EnumsManager.WeaponType.SHOTGUN when IsWeaponUnlocked(PlayerPrefsKeys.IS_SHOTGUN_UNLOCKED):
                costInfoText.text = "";
                costInfoText.gameObject.SetActive(false);
                break;
            case EnumsManager.WeaponType.RIFLE when IsWeaponUnlocked(PlayerPrefsKeys.IS_RIFLE_UNLOCKED):
                costInfoText.text = "";
                costInfoText.gameObject.SetActive(false);
                break;
            default:
                // Otherwise, display the initial value of the cost information text
                costInfoText.text = intialCostInfoText;
                costInfoText.gameObject.SetActive(true);
                break;
        }
    }

    void ModifyImage() {
        PlayerShooting playerShooting = GameObject.FindGameObjectWithTag(Tags.PLAYER).GetComponentInChildren<PlayerShooting>();

        Color imageColor = GetComponent<Image>().color;
        if (playerShooting.currentWeaponType == weaponType) {
            imageColor.a = 0.5f;
            GetComponent<Image>().sprite = highlightedImage;
        } else {
            imageColor.a = 1f;
            GetComponent<Image>().sprite = initialImage;
        }
        GetComponent<Image>().color = imageColor;
    }

    // Helper method for checking if the building is already unlocked based on the player preferences key
    bool IsWeaponUnlocked(string playerPrefsKey) => PlayerPrefs.GetInt(playerPrefsKey) == PlayerPrefsValues.TRUE;

    // Helper method for checking if the player has enough gems to unlock the building
    bool CanUnlock() => GemsSystem.Instance.currentGems >= unlockCost;

    public void Unlock() {
        // If the player doesn't have enough gems to unlock the building, then return
        if (!CanUnlock()) {
            SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.BUTTON_DISABLED);
            return;
        } 

        // Update the player preferences based on the building type
        if (weaponType == EnumsManager.WeaponType.SHOTGUN && !IsWeaponUnlocked(PlayerPrefsKeys.IS_SHOTGUN_UNLOCKED)) {
            PlayerPrefs.SetInt(PlayerPrefsKeys.IS_SHOTGUN_UNLOCKED, PlayerPrefsValues.TRUE);
        } else if (weaponType == EnumsManager.WeaponType.RIFLE && !IsWeaponUnlocked(PlayerPrefsKeys.IS_RIFLE_UNLOCKED)) {
            PlayerPrefs.SetInt(PlayerPrefsKeys.IS_RIFLE_UNLOCKED, PlayerPrefsValues.TRUE);
        } else {
            return;
        }

        SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.BUY_BUILDING);

        // Deduct the unlock cost from the player's gems
        GemsSystem.Instance.SubstractGems(unlockCost);

        // Hide the cost information text in the UI
        costInfoText.text = "";
        costInfoText.gameObject.SetActive(false);
    }

    public void SwitchWeapon() {
        PlayerShooting playerShooting = GameObject.FindGameObjectWithTag(Tags.PLAYER).GetComponentInChildren<PlayerShooting>();
        
        if (weaponType == EnumsManager.WeaponType.DEFAULT) {
            SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.BUTTON_CLICK);
            PlayerPrefs.SetInt(PlayerPrefsKeys.WEAPON, PlayerPrefsValues.WEAPON_DEFAULT);
            playerShooting.WeaponSwitch(EnumsManager.WeaponType.DEFAULT);
        } else if (weaponType == EnumsManager.WeaponType.SHOTGUN && IsWeaponUnlocked(PlayerPrefsKeys.IS_SHOTGUN_UNLOCKED)) {
            SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.BUTTON_CLICK);
            PlayerPrefs.SetInt(PlayerPrefsKeys.WEAPON, PlayerPrefsValues.WEAPON_SHOTGUN);
            playerShooting.WeaponSwitch(EnumsManager.WeaponType.SHOTGUN);
        } else if (weaponType == EnumsManager.WeaponType.RIFLE && IsWeaponUnlocked(PlayerPrefsKeys.IS_RIFLE_UNLOCKED)) {
            SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.BUTTON_CLICK);
            PlayerPrefs.SetInt(PlayerPrefsKeys.WEAPON, PlayerPrefsValues.WEAPON_RIFLE);
            playerShooting.WeaponSwitch(EnumsManager.WeaponType.RIFLE);
        }
    }
}
