using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinSystem : MonoBehaviour {
    public static CoinSystem Instance { get; private set; }
    private int currentCoin;
    [SerializeField] private TextMeshProUGUI coinInfoText;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public int GetCurrentCoin() => currentCoin;
    public void AddCoin(int amount) => currentCoin += amount;
    public void SubstractCoin(int amount) => currentCoin -= amount;

    void Update() {
        SetCoinInfo();
    }

    void SetCoinInfo() {
        coinInfoText.text = $"COIN: {currentCoin}";
	}
}
