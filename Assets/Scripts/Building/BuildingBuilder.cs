using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingBuilder : MonoBehaviour {
    [SerializeField] private EnumsManager.BuildingType buildingType;
    [SerializeField] private GameObject building;
    [SerializeField] private Transform buildingParent;
    private GameObject currentBuilding;
    [SerializeField] private int buildCost;
    [SerializeField] private bool isBuildingLocked;
    private string initialTextInfo;

    void Awake ()  {
        initialTextInfo = GetComponentInChildren<TextMeshProUGUI>().text;
    }

    void Update() {
        ModifyImageColorAlpha();
        SetLockedInfoText();
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

    void SetLockedInfoText() {
        if (isBuildingLocked) {
            GetComponentInChildren<TextMeshProUGUI>().text = "LOCKED";
        } else {
            GetComponentInChildren<TextMeshProUGUI>().text = initialTextInfo;
        }
    }

    bool CanBuild() {
        switch (buildingType) {
            case EnumsManager.BuildingType.THORN_MINE when IsBuildingLocked(PlayerPrefsKeys.IS_THORNMINE_UNLOCKED):
                return false;
            case EnumsManager.BuildingType.DECOY when IsBuildingLocked(PlayerPrefsKeys.IS_DECOY_UNLOCKED):
                return false;
            case EnumsManager.BuildingType.CANON when IsBuildingLocked(PlayerPrefsKeys.IS_CANON_UNLOCKED):
                return false;
        }

        isBuildingLocked = false;
        return CoinSystem.Instance.GetCurrentCoin() >= buildCost;
    } 

    bool IsBuildingLocked(string playerPrefsKey) => PlayerPrefs.GetInt(playerPrefsKey) == PlayerPrefsValues.FALSE;

    void OnMouseEnter() {
        Player.Instance.SetPlayerState(EnumsManager.PlayerState.BUILD);
    }

    void OnMouseExit() {
        Player.Instance.SetPlayerState(EnumsManager.PlayerState.SHOOT);
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
