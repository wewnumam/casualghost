using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSystem : MonoBehaviour {
    [SerializeField] private EnumsManager.WeaponType weaponType;

    void Update() {
        ModifyImageColorAlpha();
    }

    public void SwitchWeapon() {
        PlayerShooting playerShooting = GameObject.FindGameObjectWithTag(Tags.PLAYER).GetComponentInChildren<PlayerShooting>();
        
        if (weaponType == EnumsManager.WeaponType.DEFAULT) {
            PlayerPrefs.SetInt(PlayerPrefsKeys.WEAPON, PlayerPrefsValues.WEAPON_DEFAULT);
            playerShooting.WeaponSwitch(PlayerPrefsValues.WEAPON_DEFAULT);
        } else if (weaponType == EnumsManager.WeaponType.SHOTGUN) {
            PlayerPrefs.SetInt(PlayerPrefsKeys.WEAPON, PlayerPrefsValues.WEAPON_SHOTGUN);
            playerShooting.WeaponSwitch(PlayerPrefsValues.WEAPON_SHOTGUN);
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
}
