using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public static Player Instance { get;  private set; }
    private PlayerState playerState;

    [Header("UI Properties")]
    [SerializeField] private TextMesh playerInfoText;
    private bool canAttacked = true;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        playerState = PlayerState.SHOOT;
    }

    void Update() {
        // Player health check
        if (GetComponent<HealthSystem>().currentHealth <= 0 && GameManager.Instance.IsGameStateGameplay()) {
            GamePanelManager.Instance.GameOver();
        }

        SetPlayerInfo();
    }

    public void SetPlayerState(PlayerState playerState) => this.playerState = playerState;
    public bool IsPlayerStateShoot() => playerState == PlayerState.SHOOT;

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

public enum PlayerState {
    SHOOT,
    BUILD
}