using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("Player Properties")]
    private bool canAttacked = true;
    [SerializeField] private TextMesh playerInfoText;
    private int coin;
    [SerializeField] private TextMeshProUGUI coinInfoText;

    void Update() {
        if (GetComponent<HealthSystem>().currentHealth == 0) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SetPlayerInfo();
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag(Tags.COIN)) {
            coin++;
			Destroy(collision.gameObject);
		}
    }

    void OnCollisionStay2D(Collision2D collision) {
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
        coinInfoText.text = $"COIN: {coin}";
	}
}
