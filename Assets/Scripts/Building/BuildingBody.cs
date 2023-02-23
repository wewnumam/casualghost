using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBody : MonoBehaviour
{
    [Header("UI Properties")]
    [SerializeField] private bool isBanyan;
    [SerializeField] private TextMesh buildingInfoText;
    private bool canAttacked = true;

    void Update() {
        // BuildingBody health check
        if (GetComponent<HealthSystem>().currentHealth <= 0 && GameManager.Instance.IsGameStateGameplay()) {
            if (isBanyan) {
                GamePanelManager.Instance.GameOver();
            } else {
                Destroy(gameObject);
            }
        }

        SetBuildingInfo();
    }

    void OnCollisionStay2D(Collision2D collision) {
        // BuildingBody attacked by enemy
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

    void SetBuildingInfo() {
		buildingInfoText.text = $"HEALTH: {GetComponent<HealthSystem>().currentHealth.ToString()}";
	}
}
