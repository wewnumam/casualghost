using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPanel : MonoBehaviour {
    [SerializeField] private GameObject inventoryPanel;

    void Start()
    {
        inventoryPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }
}
