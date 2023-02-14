using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    private bool canAttacked = true;
    [Header("UI Properties")]
    private int coin;
    [SerializeField] private TextMesh playerInfoText;
    [SerializeField] private TextMeshProUGUI coinInfoText;
    [SerializeField] private GameObject inventoryPanel;

    void Start() {
        inventoryPanel.SetActive(false);
    }

    void Update() {
        if (GetComponent<HealthSystem>().currentHealth == 0) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if (Input.GetKeyDown(KeyCode.F)) {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
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
