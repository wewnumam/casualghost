using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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
    [SerializeField] private GameObject powerUpParticlePrefab;
    [HideInInspector] public bool isPowerUp;

    [Header("Player Environment Properties")]
    [SerializeField] private Light2D playerLight;
    private float initialPlayerLightIntensity;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        playerState = EnumsManager.PlayerState.SHOOT;
        initialPlayerLightIntensity = playerLight.intensity;
    }

    void Update() {
        if (GameManager.Instance.IsGameStateMainMenu()) {
            playerLight.intensity = 0;
        } else {
            playerLight.intensity = initialPlayerLightIntensity;
        }

        if (GetComponent<HealthSystem>().IsDie() && GameManager.Instance.IsGameStateGameplay()) {
            SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.PLAYER_DIE);
            foreach (var hand in playerHands) {
                Destroy(hand);
            }
            if (playerBody != null) Destroy(playerBody.gameObject);
            GetComponent<Animator>().Play(AnimationTags.PLAYER_DIE);
        }

        if (isPowerUp) {
            StartCoroutine(PowerUpAnimation());
            isPowerUp = false;
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

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.CompareTag(Tags.MIST)) {
            StartCoroutine(Attacked(.5f, 0f));
		}
    }

	IEnumerator Attacked(float damageAmount, float waitForSeconds) {
        SoundManager.Instance.PlaySound(UtilsClass.SuffleSFX(new EnumsManager.SoundEffect[] {
            EnumsManager.SoundEffect.PLAYER_HURT_1,
            EnumsManager.SoundEffect.PLAYER_HURT_2,
            EnumsManager.SoundEffect.PLAYER_HURT_3,
            EnumsManager.SoundEffect.PLAYER_HURT_4
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

    IEnumerator PowerUpAnimation() {
        yield return new WaitForSeconds(1f);
        GameObject powerUp = Instantiate(powerUpParticlePrefab, transform);
        Destroy(powerUp, 3f);
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

