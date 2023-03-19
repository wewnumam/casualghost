using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class BuildingBody : MonoBehaviour {
    [Tooltip("The game will be over if the banyan dies")]
    [SerializeField] private bool isBanyan;

    [Header("UI Properties")]
    [SerializeField] private TextMesh buildingInfoText;

    private bool canAttacked = true;

    void Update() {
        if (GetComponent<HealthSystem>().IsDie() && GameManager.Instance.IsGameStateGameplay()) {
            if (isBanyan) {
                SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.PLAYER_DIE);
                GamePanelManager.Instance.GameOver();
            } else {
                Destroy(gameObject);
            }
        }

        // SetBuildingInfo();
    }

    void SetBuildingInfo() {
		buildingInfoText.text = $"HEALTH: {GetComponent<HealthSystem>().currentHealth.ToString()}";
	}

    void OnCollisionStay2D(Collision2D collision) {
        // BuildingBody attacked by enemy
		if (collision.gameObject.CompareTag(Tags.ENEMY) && canAttacked) {
			StartCoroutine(Attacked(1f, 1f));
		}
	}

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.CompareTag(Tags.MIST)) {
            StartCoroutine(Attacked(.5f, 0f));
		}
    }

	IEnumerator Attacked(float damageAmount, float waitForSeconds) {
        if (isBanyan) {
            SoundManager.Instance.PlaySound(UtilsClass.SuffleSFX(new EnumsManager.SoundEffect[] {
                EnumsManager.SoundEffect.PLAYER_HURT_1,
                EnumsManager.SoundEffect.PLAYER_HURT_2,
                EnumsManager.SoundEffect.PLAYER_HURT_3,
                EnumsManager.SoundEffect.PLAYER_HURT_4
            }));
            CameraShaker.Instance.ShakeOnce(10f, 10f, 0f, .25f);
        }
        canAttacked = false; // Prevent enemy attack during the delay
        yield return new WaitForSeconds(waitForSeconds);
		GetComponent<HealthSystem>().TakeDamage(damageAmount);
        GetComponent<Animator>().Play("BuildingHurt");
        canAttacked = true; // Allow enemy attack again
	}
}
