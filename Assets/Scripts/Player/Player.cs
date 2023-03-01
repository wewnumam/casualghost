using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public static Player Instance { get;  private set; }

    [Header("Player State Properties")]
    private EnumsManager.PlayerState playerState;
    private bool canAttacked = true;

    [Header("UI Properties")]
    [SerializeField] private TextMesh playerInfoText;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        playerState = EnumsManager.PlayerState.SHOOT;
    }

    void Update() {
        if (GetComponent<HealthSystem>().IsDie() && GameManager.Instance.IsGameStateGameplay()) {
            GamePanelManager.Instance.GameOver();
        }

        SetPlayerInfo();
    }

    public void SetPlayerState(EnumsManager.PlayerState playerState) => this.playerState = playerState;
    public bool IsPlayerStateShoot() => playerState == EnumsManager.PlayerState.SHOOT;

    void OnCollisionEnter2D(Collision2D collision) {
        // Player collect coin
        if (collision.gameObject.CompareTag(Tags.COIN)) {
            CoinSystem.Instance.AddCoin(1);
			Destroy(collision.gameObject);
		}

        // Player shoot by enemy
        if (collision.gameObject.CompareTag(Tags.ENEMY_BULLET)) {
            StartCoroutine(Attacked(collision.gameObject.GetComponent<Projectile>().bulletDamage, 0f));
		}
    }

    void OnCollisionStay2D(Collision2D collision) {
        // Player attacked by enemy
		if (collision.gameObject.CompareTag(Tags.ENEMY) && canAttacked) {
			StartCoroutine(Attacked(1f, 1f));
		}
	}

	IEnumerator Attacked(float damageAmount, float waitForSeconds) {
        canAttacked = false; // Prevent enemy attack during the delay
        yield return new WaitForSeconds(waitForSeconds);
        GetComponent<Animator>().Play(AnimationTags.PLAYER_HURT);
		GetComponent<HealthSystem>().TakeDamage(damageAmount);
        canAttacked = true; // Allow enemy attack again
	}

    void SetPlayerInfo() {
		playerInfoText.text = $"HEALTH: {GetComponent<HealthSystem>().currentHealth.ToString()}\nBULLET: {GetComponentInChildren<PlayerShooting>().roundsLeft.ToString()}";
	}
}

