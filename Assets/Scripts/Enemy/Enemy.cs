using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Enemy : MonoBehaviour {
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private float _damageAmount;
    public float damageAmount { get => _damageAmount; }
    [SerializeField] private float _attackSpeed;
    public float attackSpeed { get => _attackSpeed; }

    [Header("Coin Instantiate Properties")]
    [SerializeField] private GameObject coinPrefab;
    
    [Header("Particle Instantiate Properties")]
    [SerializeField] private GameObject particlePrefab;

    private enum EnemyType {
        DEFAULT,
        RUNNER,
        GIANT,
        SHOOTER,
        BOSS
    }

    void Start() {
        switch (enemyType) {
            case EnemyType.DEFAULT:
                SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.ENEMY_SPAWN_DEFAULT);
                break;
            case EnemyType.RUNNER:
                SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.ENEMY_SPAWN_SMALL);
                break;
            case EnemyType.GIANT:
                SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.ENEMY_SPAWN_BIG);
                break;
            case EnemyType.SHOOTER:
                SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.ENEMY_SPAWN_DEFAULT);
                break;
            case EnemyType.BOSS:
                SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.ENEMY_SPAWN_BIG);
                break;
        }
    }

    void Update() {
        if (GetComponent<HealthSystem>().IsDie()) {
            if (GetComponent<ShadowCaster2D>() != null) {
                GetComponent<ShadowCaster2D>().enabled = false;
            }
            if (GetComponents<BoxCollider2D>() != null) {
                foreach (var collider in GetComponents<BoxCollider2D>()) {
                    collider.enabled = false;
                }
            }
            PlayEnemyDieAnimation();
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag(Tags.TREE) && IsEnemyTypeBoss()) {
            Destroy(collision.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        // Enemy take damage from bullet
        if (collider.gameObject.CompareTag(Tags.BULLET_TYPE_ONE) || collider.gameObject.CompareTag(Tags.BULLET_TYPE_TWO)) {
            SoundManager.Instance.PlaySound(UtilsClass.SuffleSFX(new EnumsManager.SoundEffect[] {
                EnumsManager.SoundEffect.ENEMY_BLOOD_1,
                EnumsManager.SoundEffect.ENEMY_BLOOD_2,
                EnumsManager.SoundEffect.ENEMY_BLOOD_3
            }));
            float bulletDamage = collider.gameObject.GetComponent<Projectile>().bulletDamage;
            GetComponent<HealthSystem>().TakeDamage(bulletDamage);
            GetComponent<FloatingText>().InstantiateFloatingText((bulletDamage * 100).ToString(), transform);
        }

        if (collider.gameObject.CompareTag(Tags.EXPLOSION)) {
            const float EXPLOSION_DAMAGE = .25f; 
            GetComponent<HealthSystem>().TakeDamage(EXPLOSION_DAMAGE);
            GetComponent<FloatingText>().InstantiateFloatingText((EXPLOSION_DAMAGE * 100).ToString(), transform);
        }

        if (collider.gameObject.CompareTag(Tags.ROOT) ||
            collider.gameObject.CompareTag(Tags.THORN_MINE) ||
            collider.gameObject.CompareTag(Tags.DECOY) ||
            collider.gameObject.CompareTag(Tags.CANNON)) {
            if (IsEnemyTypeBoss()) {
                Destroy(collider.gameObject);
            }
        }

        if (collider.gameObject.CompareTag(Tags.THORN_MINE) && IsEnemyTypeRunner()) {
            Destroy(collider.gameObject);
            PlayEnemyDieAnimation();
        }
    }

    void OnMouseEnter() {
        GameCursor.Instance.SetEnemyCursor();
    }

    void OnMouseExit() {
        GameCursor.Instance.SetDefaultCursor();
    }

    public bool IsEnemyTypeDefault() => enemyType == EnemyType.DEFAULT;
    public bool IsEnemyTypeRunner() => enemyType == EnemyType.RUNNER;
    public bool IsEnemyTypeGiant() => enemyType == EnemyType.GIANT;
    public bool IsEnemyTypeShooter() => enemyType == EnemyType.SHOOTER;
    public bool IsEnemyTypeBoss() => enemyType == EnemyType.BOSS;

    public void PlayEnemyIdleAnimation() {
        if (IsEnemyTypeDefault()) {
            GetComponent<Animator>().Play(AnimationTags.ENEMY_DEFAULT_IDLE);
        }
    }

    public void PlayEnemyWalkAnimation() {
        if (GetComponent<HealthSystem>().IsDie()) return;

        if (IsEnemyTypeDefault()) {
            GetComponent<Animator>().Play(AnimationTags.ENEMY_DEFAULT_WALK);
        }
    }

    public void PlayEnemyHurtAnimation() {
        if (GetComponent<HealthSystem>().IsDie()) return;

        if (IsEnemyTypeDefault()) {
            GetComponent<Animator>().Play(AnimationTags.ENEMY_DEFAULT_HURT);
        }
    }

    public void PlayEnemyAttackAnimation() {
        if (GetComponent<HealthSystem>().IsDie()) return;

        if (IsEnemyTypeDefault()) {
            GetComponent<Animator>().Play(AnimationTags.ENEMY_DEFAULT_ATTACK);
        }
    }

    private bool isAnimationDieCalled = false;
    void PlayEnemyDieAnimation() {
        if (isAnimationDieCalled) return;
        GameObject particle = Instantiate(particlePrefab, transform.position, Quaternion.identity);

        switch (enemyType) {
            case EnemyType.DEFAULT:
                SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.ENEMY_DIE_DEFAULT);
                GetComponent<Animator>().Play(AnimationTags.ENEMY_DEFAULT_DIE);
                break;
            case EnemyType.RUNNER:
                SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.ENEMY_DIE_SMALL);
                EnemyDie();
                break;
            case EnemyType.GIANT:
                SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.ENEMY_DIE_BIG);
                EnemyDie();
                break;
            case EnemyType.SHOOTER:
                SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.ENEMY_DIE_DEFAULT);
                EnemyDie();
                break;
            case EnemyType.BOSS:
                SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.ENEMY_DIE_BIG);
                EnemyDie();
                break;  
        }

        isAnimationDieCalled = true;
    } 


    public void EnemyDie() {
        Instantiate(coinPrefab, transform.position, Quaternion.identity);
        CollectibleItem.Instance.AddEnemyKillCounter();
        if (CollectibleItem.Instance.canSpawnCollectibleItem) {
            GameObject collectibleItemObject = Instantiate(CollectibleItem.Instance.collectibleItemObject, transform.position, Quaternion.identity);
            collectibleItemObject.GetComponent<SpriteRenderer>().sprite = CollectibleItem.Instance.currentItem.icon;
            collectibleItemObject.GetComponent<SpriteRenderer>().material = CollectibleItem.Instance.currentItem.material;
            collectibleItemObject.GetComponent<CollectibleItemObject>().item = CollectibleItem.Instance.currentItem;
        }
        Destroy(gameObject);
    }
}
