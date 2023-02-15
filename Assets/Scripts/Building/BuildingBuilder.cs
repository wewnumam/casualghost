using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingBuilder : MonoBehaviour
{
    [SerializeField] private GameObject building;
    private GameObject currentBuilding;
    [SerializeField] private int buildCost;

    bool CanBuild() => CoinSystem.Instance.GetCurrentCoin() >= buildCost;

    void Update() {
        ModifyImageColorAlpha();
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

    void OnMouseDown() {
        if (CanBuild()) {
            currentBuilding = Instantiate(building, UtilsClass.GetMouseWorldPosition(), Quaternion.identity);
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
