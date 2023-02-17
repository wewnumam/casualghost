using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [Header("UI Properties")]
    [SerializeField] private TextMesh playerInfoText;
    private bool canAttacked = true;

    void Update() {
        // Player health check
        if (GetComponent<HealthSystem>().currentHealth <= 0) {
            GamePanelManager.Instance.GameOver();
        }

        SetPlayerInfo();
    }

    void OnCollisionEnter2D(Collision2D collision) {
        // Player collect coin
        if (collision.gameObject.CompareTag(Tags.COIN)) {
            CoinSystem.Instance.AddCoin(1);
			Destroy(collision.gameObject);
		}
    }

    void OnCollisionStay2D(Collision2D collision) {
        // Player attacked by enemy
		if (collision.gameObject.CompareTag(Tags.ENEMY) && canAttacked) {
			StartCoroutine(Attacked(1f, 1f));
		}
	}

	IEnumerator Attacked(float damageAmount, float waitForSeconds) {
        canAttacked = false;
        yield return new WaitForSeconds(waitForSeconds);
        GetComponent<Animator>().Play(AnimationTags.PLAYER_HURT);
		GetComponent<HealthSystem>().TakeDamage(damageAmount);
        canAttacked = true;
	}

    void SetPlayerInfo() {
		playerInfoText.text = $"HEALTH: {GetComponent<HealthSystem>().currentHealth.ToString()}\nBULLET: {GetComponentInChildren<PlayerShooting>().roundsLeft.ToString()}";
	}
}
