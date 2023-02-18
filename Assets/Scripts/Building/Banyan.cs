using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banyan : MonoBehaviour
{
    [Header("UI Properties")]
    [SerializeField] private TextMesh banyanInfoText;
    private bool canAttacked = true;

    void Update() {
        // Banyan health check
        if (GetComponent<HealthSystem>().currentHealth <= 0 && GameManager.Instance.IsGameStateGameplay()) {
            GamePanelManager.Instance.GameOver();
        }

        SetBanyanInfo();
    }

    void OnCollisionStay2D(Collision2D collision) {
        // Banyan attacked by enemy
		if (collision.gameObject.CompareTag(Tags.ENEMY) && canAttacked) {
			StartCoroutine(Attacked(1f, 1f));
		}
	}

	IEnumerator Attacked(float damageAmount, float waitForSeconds) {
        canAttacked = false;
        yield return new WaitForSeconds(waitForSeconds);
		GetComponent<HealthSystem>().TakeDamage(damageAmount);
        canAttacked = true;
	}

    void SetBanyanInfo() {
		banyanInfoText.text = $"HEALTH: {GetComponent<HealthSystem>().currentHealth.ToString()}";
	}
}
