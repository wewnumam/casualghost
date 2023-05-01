using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSystem : MonoBehaviour {
   [SerializeField] private EnumsManager.PlayerType playerType;

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

    void Start() {
        if (IsUnlocked()) {
            costInfoText.text = "";
            costInfoText.gameObject.SetActive(false);
        }    
    }

    void Update() {
        SetCostInfoText();
        ModifyImage();
    }

    void SetCostInfoText() {
        if (IsUnlocked()) {
            costInfoText.text = "";
            costInfoText.gameObject.SetActive(false);
        } else {
            costInfoText.text = intialCostInfoText;
            costInfoText.gameObject.SetActive(true);
        }
    }

    void ModifyImage() {
        Color imageColor = GetComponent<Image>().color;
        if (Player.Instance.playerType == playerType) {
            imageColor.a = 0.5f;
            GetComponent<Image>().sprite = highlightedImage;
        } else {
            imageColor.a = 1f;
            GetComponent<Image>().sprite = initialImage;
        }
        GetComponent<Image>().color = imageColor;
    }

    // Helper method for checking if the player has enough gems to unlock the building
    bool IsUnlocked() => MissionManager.Instance.missionProgressValue >= unlockCost;

    public void SwitchPlayer(GameObject playerPrefab) {
        if (IsUnlocked()) {
            SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.BUTTON_CLICK);
            PlayerManager.Instance.ReplacePlayer(playerPrefab);
            ModifyImage();
        } else {
            SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.BUTTON_DISABLED);
        }
    }
}
