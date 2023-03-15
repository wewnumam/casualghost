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

    [Header("Player Skill Properties")]
    [SerializeField] private GameObject explosionPrefab;
    private bool isBreathRoomActive;

    [Header("Player Animation Properties")]
    [SerializeField] private Animator playerBody;
    [SerializeField] private GameObject[] playerHands;
    [HideInInspector] public bool isAttacked;

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
            foreach (var hand in playerHands) {
                Destroy(hand);
            }
            if (playerBody != null) Destroy(playerBody.gameObject);
            GetComponent<Animator>().Play(AnimationTags.PLAYER_DIE);
        }

        SetPlayerInfo();
    }

    public void SetPlayerState(EnumsManager.PlayerState playerState) => this.playerState = playerState;
    public void SetBreathRoomActive() => isBreathRoomActive = true;
    public bool IsPlayerStateShoot() => playerState == EnumsManager.PlayerState.SHOOT;

    void OnCollisionEnter2D(Collision2D collision) {
        // Player collect coin
        if (collision.gameObject.CompareTag(Tags.COIN)) {
            SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.COLLECT_COIN);
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
            collision.gameObject.GetComponent<Enemy>().PlayEnemyAttackAnimation();
			StartCoroutine(Attacked(1f, 1f));
		}
	}

	IEnumerator Attacked(float damageAmount, float waitForSeconds) {
        SoundManager.Instance.PlaySound(UtilsClass.SuffleSFX(new EnumsManager.SoundEffect[] {
            EnumsManager.SoundEffect.PLAYER_HURT_1,
            EnumsManager.SoundEffect.PLAYER_HURT_2,
            EnumsManager.SoundEffect.PLAYER_HURT_3
        }));
        isAttacked = true;
        canAttacked = false; // Prevent enemy attack during the delay
        yield return new WaitForSeconds(waitForSeconds);
		GetComponent<HealthSystem>().TakeDamage(damageAmount);
        canAttacked = true; // Allow enemy attack again
        isAttacked = false;
        if (isBreathRoomActive) {
            GameObject explosion = Instantiate(explosionPrefab, transform);
            yield return new WaitForSeconds(waitForSeconds);
            Destroy(explosion);
        }
	}

    void SetPlayerInfo() {
        if (GetComponent<HealthSystem>().IsDie()) return;

        playerInfoText.text = "";
		// playerInfoText.text += $"HEALTH: {GetComponent<HealthSystem>().currentHealth.ToString()}\n";
        playerInfoText.text += $"BULLET: {GetComponentInChildren<PlayerShooting>().roundsLeft.ToString()}";
	}

    public void SetGameOver() {
        GamePanelManager.Instance.GameOver();
    }
}

