using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Rendering.Universal;

public class BuildingBuilder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IDragHandler, IPointerUpHandler {
    [SerializeField] private EnumsManager.BuildingType buildingType;

    [Header("Instantiate Properties")]
    [SerializeField] private GameObject building;
    [SerializeField] private Transform buildingParent;
    private GameObject currentBuilding;

    [Header("Economic Settings")]
    [SerializeField] private int buildCost;
    [SerializeField] private bool isBuildingLocked;

    [Header("UI Properties")]
    private string initialTextInfo;
    private float initialFontSize;
    private float initialLightIntensity;
    private HorizontalLayoutGroup costContainer;

    [Header("Particle Properties")]
    [SerializeField] private GameObject particleDropBuilding; 

    void Awake ()  {
        // Get the initial text of the UI element that displays information about the building.
        initialTextInfo = GetComponentInChildren<TextMeshProUGUI>().text;
        initialFontSize = GetComponentInChildren<TextMeshProUGUI>().fontSize;
        initialLightIntensity = GetComponentInChildren<Light2D>().intensity;
        costContainer = GetComponentInChildren<HorizontalLayoutGroup>();
    }

    void Update() {
        // Update the image color and locked info text of the building UI element every frame.
        ModifyImageColorAlpha();
        SetLockedInfoText();
    }

    void ModifyImageColorAlpha() {
        // Modify the alpha value of the image color of the building UI element based on whether the player can afford to build it.
        Color imageColor = GetComponent<Image>().color;
        if (CanBuild()) {
            imageColor.a = 1f;
            GetComponentInChildren<Light2D>().intensity = initialLightIntensity;
        } else {
            GetComponentInChildren<Light2D>().intensity = 0;
            imageColor.a = 0.5f;
        }
        GetComponent<Image>().color = imageColor;
    }

    void SetLockedInfoText() {
        // Set the text of the UI element that displays information about the building to either "LOCKED" or the initial text, depending on whether the building is locked.
        if (isBuildingLocked) {
            GetComponentInChildren<HorizontalLayoutGroup>().padding.left = 0;
            GetComponentInChildren<HorizontalLayoutGroup>().spacing= -30;
            GetComponentInChildren<TextMeshProUGUI>().text = "LOCKED";
            GetComponentInChildren<TextMeshProUGUI>().fontSize = 22;
        } else {
            GetComponentInChildren<HorizontalLayoutGroup>().padding.left = 40;
            GetComponentInChildren<HorizontalLayoutGroup>().spacing = 0;
            GetComponentInChildren<TextMeshProUGUI>().text = initialTextInfo;
            GetComponentInChildren<TextMeshProUGUI>().fontSize = initialFontSize;
        }
    }

    bool CanBuild() {
        // Check if the player can afford to build the current building type, and return true or false accordingly.
        switch (buildingType) {
            case EnumsManager.BuildingType.THORN_MINE when IsBuildingLocked(PlayerPrefsKeys.IS_THORNMINE_UNLOCKED):
                return false;
            case EnumsManager.BuildingType.DECOY when IsBuildingLocked(PlayerPrefsKeys.IS_DECOY_UNLOCKED):
                return false;
            case EnumsManager.BuildingType.CANON when IsBuildingLocked(PlayerPrefsKeys.IS_CANNON_UNLOCKED):
                return false;
        }

        // If the building is not locked and the player can afford to build it, return true.
        isBuildingLocked = false;
        return CoinSystem.Instance.GetCurrentCoin() >= buildCost;
    } 

    bool IsBuildingLocked(string playerPrefsKey) => PlayerPrefs.GetInt(playerPrefsKey) == PlayerPrefsValues.FALSE;

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)  {
        // When the mouse enters the building UI element, set the player's state to BUILD.
        Player.Instance.SetPlayerState(EnumsManager.PlayerState.BUILD);
        GameCursor.Instance.SetBuildCursor();
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData) {
        // When the mouse exits the building UI element, set the player's state to SHOOT.
        Player.Instance.SetPlayerState(EnumsManager.PlayerState.SHOOT);
        GameCursor.Instance.SetDefaultCursor();
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData) {
        // When the mouse is pressed down on the building UI element, instantiate the building at the mouse's current world position if the player can afford to build it.
        if (CanBuild()) {
            currentBuilding = Instantiate(building, UtilsClass.GetMouseWorldPosition(), Quaternion.identity, buildingParent);
        }
    }

    void IDragHandler.OnDrag(PointerEventData eventData) {
        // While the mouse is being dragged on the building UI element, move the current building to the mouse's current world position if the player can afford to build it.
        if (CanBuild() && currentBuilding != null) {
            currentBuilding.transform.position = UtilsClass.GetMouseWorldPosition();
            GameCursor.Instance.SetBuildCursor();
        }
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData) {
        // When the mouse is pressed up on the building UI element, subtracts the building cost from the current coin value if the building can be built.
        if (CanBuild()) {
            CoinSystem.Instance.SubstractCoin(buildCost);
            SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.BUILDING_DROP);
            GameCursor.Instance.SetDefaultCursor();
            GameObject ps = Instantiate(
				particleDropBuilding,
				UtilsClass.GetMouseWorldPosition(),
				transform.rotation
			);
			ps.GetComponentInChildren<ParticleSystem>().Play();
        }
    }
}
