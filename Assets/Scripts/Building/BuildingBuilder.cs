using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBuilder : MonoBehaviour
{
    [SerializeField] private GameObject building;
    private GameObject currentBuilding;

    void OnMouseDown() {
        currentBuilding = Instantiate(building, UtilsClass.GetMouseWorldPosition(), Quaternion.identity);
    }

    void OnMouseDrag() {
        currentBuilding.transform.position = UtilsClass.GetMouseWorldPosition();
    }
}
