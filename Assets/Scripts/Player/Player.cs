using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("UI Properties")]
    [SerializeField] private TextMesh playerInfoText;
    private bool canAttacked = true;

    void Update() {
        // Player health check
        if (GetComponent<HealthSystem>().currentHealth == 0) SceneManager.LoadScene(SceneManager.GetActiveScene().name);

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
			StartCoroutine(Attacked());
		}
	}

	IEnumerator Attacked() {
        canAttacked = false;
        yield return new WaitForSeconds(1f);
		GetComponent<HealthSystem>().TakeDamage(1);
        canAttacked = true;
	}

    void SetPlayerInfo() {
		playerInfoText.text = $"HEALTH: {GetComponent<HealthSystem>().currentHealth.ToString()}\nBULLET: {GetComponentInChildren<PlayerShooting>().roundsLeft.ToString()}";
	}
}
