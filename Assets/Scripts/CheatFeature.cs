using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatFeature : MonoBehaviour {
    [SerializeField] float fastForwardPlayTimeBy;
    [SerializeField] int addCoinAmount;

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
        GameObject player = GameObject.FindWithTag(Tags.PLAYER);
        player.GetComponent<HealthSystem>().Heal(player.GetComponent<HealthSystem>().maxHealth - player.GetComponent<HealthSystem>().currentHealth);
    }

    void HealBanyan() {
        GameObject banyan = GameObject.FindWithTag(Tags.BANYAN);
        banyan.GetComponent<HealthSystem>().Heal(banyan.GetComponent<HealthSystem>().maxHealth - banyan.GetComponent<HealthSystem>().currentHealth);
    }
}