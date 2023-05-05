using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatFeature : MonoBehaviour {
    [SerializeField] private float fastForwardPlayTimeBy;
    [SerializeField] private int addCoinAmount;
    [SerializeField] private int addMissionProgressAmount;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Minus)) {
            FastForwardPlayTime(fastForwardPlayTimeBy);   
        }
        
        if (Input.GetKeyDown(KeyCode.Equals)) {
            AddCoin(addCoinAmount);
        }

        if (Input.GetKeyDown(KeyCode.Delete)) {
            ClearAllEnemyOnScren();
        }
        
        if (Input.GetKeyDown(KeyCode.LeftBracket)) {
            HealPlayer();
        }

        if (Input.GetKeyDown(KeyCode.RightBracket)) {
            HealBanyan();
        }

        if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0)) {
            MissionManager.Instance.AddMissionProgress(addMissionProgressAmount);
        }
    }

    void FastForwardPlayTime(float fastForwardPlayTimeBy) {
        GameManager.Instance.FastForwardPlayTime(fastForwardPlayTimeBy);
    }

    void AddCoin(int amount) {
        CoinSystem.Instance.AddCoin(amount);
    }

    void ClearAllEnemyOnScren() {
        foreach (var enemy in GameManager.Instance.currentEnemies) {
            Destroy(enemy);
        }
    }

    void HealPlayer() {
        Player.Instance.healthSystem.Heal(Player.Instance.healthSystem.maxHealth - Player.Instance.healthSystem.currentHealth);
    }

    void HealBanyan() {
        BanyanDefenseManager.Instance.healthSystem.Heal(BanyanDefenseManager.Instance.healthSystem.maxHealth - BanyanDefenseManager.Instance.healthSystem.currentHealth);
    }
}