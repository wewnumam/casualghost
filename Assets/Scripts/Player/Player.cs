using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour {
    public static Player Instance { get;  private set; }

    [SerializeField] private EnumsManager.PlayerType _playerType;
    public EnumsManager.PlayerType playerType { get => _playerType; }

    [Header("Caching Components")]
    public HealthSystem healthSystem;
    public FloatingText floatingText;
    public Animator animator;
    public PlayerMovement playerMovement;
    public PlayerShooting playerShooting;
    public CircleCollider2D coinCollection;
    

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
    private bool hasPlayerDyingSFXBeenCalled; 

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

        if (healthSystem.IsDie() && GameManager.Instance.IsGameStateGameplay()) {
            foreach (var hand in playerHands) {
                Destroy(hand);
            }
            if (playerBody != null) Destroy(playerBody.gameObject);
            animator.Play(AnimationTags.PLAYER_DIE);
            playerMovement.enabled = false;
        }

        if (isPowerUp) {
            StartCoroutine(PowerUpAnimation());
            isPowerUp = false;
        }

        if (!healthSystem.IsDying() && !BanyanDefenseManager.Instance.healthSystem.IsDying()) {
            PostProcessingEffect.Instance.ResetDyingEffect();
            SoundManager.Instance.StopSound(EnumsManager.SoundEffect.PLAYER_DYING);
            hasPlayerDyingSFXBeenCalled = false;
        } else {
            if (!hasPlayerDyingSFXBeenCalled) {
                SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.PLAYER_DYING);
                hasPlayerDyingSFXBeenCalled = true;
            }
        }

        // SetPlayerInfo();
    }

    public void SetPlayerState(EnumsManager.PlayerState playerState) => this.playerState = playerState;
    public void SetBreathRoomActive() => isBreathRoomActive = true;
    public bool IsPlayerStateShoot() => playerState == EnumsManager.PlayerState.SHOOT;
    public bool IsPlayerTypeDefault() => _playerType == EnumsManager.PlayerType.DEFAULT;
    public bool IsPlayerTypeTwo() => _playerType == EnumsManager.PlayerType.TYPE_TWO;
    public bool IsPlayerTypeThree() => _playerType == EnumsManager.PlayerType.TYPE_THREE;

    void OnCollisionEnter2D(Collision2D collision) {
        // Player collect coin
        if (collision.gameObject.CompareTag(Tags.COIN)) {
            SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.COLLECT_COIN);
            CoinSystem.Instance.AddCoin(1);
			Destroy(collision.gameObject);
		}

        if (collision.gameObject.CompareTag(Tags.COLLECTIBLE_ITEM)) {
            CollectibleItem.Instance.ActivateSkill(collision.gameObject.GetComponent<CollectibleItemObject>().item);
            Destroy(collision.gameObject);
        }
    }

    void OnCollisionStay2D(Collision2D collision) {
        // Player attacked by enemy
		if (collision.gameObject.CompareTag(Tags.ENEMY) && canAttacked) {
            collision.gameObject.GetComponent<Enemy>().PlayEnemyAttackAnimation();
			StartCoroutine(Attacked(collision.gameObject.GetComponent<Enemy>().damageAmount, collision.gameObject.GetComponent<Enemy>().attackSpeed));
		}
	}

    void OnTriggerEnter2D(Collider2D collider) {
        // Player shoot by enemy
        if (collider.gameObject.CompareTag(Tags.ENEMY_BULLET)) {
            StartCoroutine(Attacked(collider.gameObject.GetComponent<Projectile>().bulletDamage, 0f));
            Destroy(collider.gameObject);
		}

        if (collider.gameObject.CompareTag(Tags.MIST)) {
            StartCoroutine(Attacked(collider.gameObject.GetComponent<Mist>().damageAmount, collider.gameObject.GetComponent<Mist>().attackSpeed));
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
		healthSystem.TakeDamage(damageAmount);
        if (healthSystem.IsDying()) {
            PostProcessingEffect.Instance.DyingEffect(damageAmount / 10);
        }
        floatingText.InstantiateFloatingText((damageAmount * 100).ToString(), transform);
        canAttacked = true; // Allow enemy attack again
        isAttacked = false;
        if (isBreathRoomActive) {
            SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.EXPLOSION);
            GameObject explosion = Instantiate(explosionPrefab, transform);
            yield return new WaitForSeconds(waitForSeconds);
            Destroy(explosion);
        }
	}

    IEnumerator PowerUpAnimation() {
        yield return new WaitForSeconds(1f);
        GameObject powerUp = Instantiate(powerUpParticlePrefab, transform);
        Destroy(powerUp, 5f);
    }

    void SetPlayerInfo() {
        if (healthSystem.IsDie()) return;

        playerInfoText.text = "";
		// playerInfoText.text += $"HEALTH: {healthSystem.currentHealth.ToString()}\n";
        playerInfoText.text += GetComponentInChildren<PlayerShooting>().roundsLeft.ToString();
	}

    public void DieSFX() {
        SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.PLAYER_DIE);
    }

    public void SetGameOver() {
        GamePanelManager.Instance.GameOver();
    }
}

