using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingBuilder : MonoBehaviour {
    [SerializeField] private BuildingType buildingType;
    [SerializeField] private GameObject building;
    [SerializeField] private Transform buildingParent;
    private GameObject currentBuilding;
    [SerializeField] private int buildCost;
    [SerializeField] private bool isBuildingLocked;
    private string initialTextInfo;

    void Awake ()  {
        initialTextInfo = GetComponentInChildren<TextMeshProUGUI>().text;
    }

    bool CanBuild() {
        if (buildingType == BuildingType.THORN_MINE && PlayerPrefs.GetInt(PlayerPrefsKeys.IS_THORNMINE_UNLOCKED) == PlayerPrefsValues.FALSE) {
            return false;
        } else if (buildingType == BuildingType.DECOY && PlayerPrefs.GetInt(PlayerPrefsKeys.IS_DECOY_UNLOCKED) == PlayerPrefsValues.FALSE) {
            return false;
        } else if (buildingType == BuildingType.CANON && PlayerPrefs.GetInt(PlayerPrefsKeys.IS_CANON_UNLOCKED) == PlayerPrefsValues.FALSE) {
            return false;
        }

        isBuildingLocked = false;
        return CoinSystem.Instance.GetCurrentCoin() >= buildCost;
    } 

    void Update() {
        ModifyImageColorAlpha();

        if (isBuildingLocked) {
            GetComponentInChildren<TextMeshProUGUI>().text = "LOCKED";
        } else {
            GetComponentInChildren<TextMeshProUGUI>().text = initialTextInfo;
        }
    }

    void ModifyImageColorAlpha() {
        Color imageColor = GetComponent<Image>().color;
        if (CanBuild()) {
            imageColor.a = 1f;
        } else {
            imageColor.a = 0.5f;
        }
        GetComponent<Image>().color = imageColor;
    }

    void OnMouseEnter() {
        Player.Instance.SetPlayerState(PlayerState.BUILD);
    }

    void OnMouseExit() {
        Player.Instance.SetPlayerState(PlayerState.SHOOT);
    }

    void OnMouseDown() {
        if (CanBuild()) {
            currentBuilding = Instantiate(building, UtilsClass.GetMouseWorldPosition(), Quaternion.identity, buildingParent);
        }
    }

    void OnMouseDrag() {
        if (CanBuild()) {
            currentBuilding.transform.position = UtilsClass.GetMouseWorldPosition();
        }
    }

    void OnMouseUp() {
        if (CanBuild()) {
            CoinSystem.Instance.SubstractCoin(buildCost);
        }
    }
}
