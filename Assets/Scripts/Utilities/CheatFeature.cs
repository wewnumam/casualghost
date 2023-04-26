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
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tags.ENEMY);
        foreach (var enemy in enemies) {
            Destroy(enemy.gameObject);
        }
    }

    void HealPlayer() {
        HealthSystem playerHealthSystem = GameObject.FindWithTag(Tags.PLAYER).GetComponent<HealthSystem>();
        playerHealthSystem.Heal(playerHealthSystem.maxHealth - playerHealthSystem.currentHealth);
    }

    void HealBanyan() {
        HealthSystem banyanHealthSystem = GameObject.FindWithTag(Tags.BANYAN).GetComponent<HealthSystem>();
        banyanHealthSystem.Heal(banyanHealthSystem.maxHealth - banyanHealthSystem.currentHealth);
    }
}