using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanyanDefenseManager : MonoBehaviour  {
    public static BanyanDefenseManager Instance { get; private set; }

    [Header("Banyan Defense Button Instantiate Properties")]
    [SerializeField] private int enemyKillAmountToSpawnCollectibleItem;
    [SerializeField] private List<GameObject> banyanDefenseButton;
    public GameObject buttonCanvas;
    [SerializeField] private Transform buttonContainer;
    public Transform parentBanyanDefenseObject;

    void Awake () {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        InstantiateRandomButton();
        buttonCanvas.SetActive(false);
    }

    void Update() {
        if (PlayerPrefs.GetInt(PlayerPrefsKeys.ENEMY_KILLED_COUNTER) % enemyKillAmountToSpawnCollectibleItem == 0 && !buttonCanvas.activeSelf) {
            buttonCanvas.SetActive(true);
        }
    }

    void InstantiateRandomButton() {
        foreach (Transform child in buttonContainer.transform) {
            Destroy(child.gameObject);
        }

        // Shuffle the list to ensure randomness
        for (int i = banyanDefenseButton.Count - 1; i > 0; i--) {
            int j = UnityEngine.Random.Range(0, i + 1);
            GameObject temp = banyanDefenseButton[i];
            banyanDefenseButton[i] = banyanDefenseButton[j];
            banyanDefenseButton[j] = temp;
        }

        // Get the first 3 values from the shuffled list
        for (int i = 0; i < 3; i++) {
            Instantiate(banyanDefenseButton[i], buttonContainer);
        }
    }
       
}
