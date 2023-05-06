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

    [Header("Caching Components")]
    public HealthSystem healthSystem;
    public ShadowCaster2D shadowCaster2D;
    public FloatingText floatingText;
    public Animator animator;
    public BoxCollider2D[] boxCollider2Ds;

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
        GameManager.Instance.currentEnemies.Add(gameObject);

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
        if (healthSystem.IsDie()) {
            if (shadowCaster2D != null) {
                shadowCaster2D.enabled = false;
            }
            if (boxCollider2Ds != null) {
                foreach (var collider in boxCollider2Ds) {
                    collider.enabled = false;
                }
            }
            PlayEnemyDieAnimation();
            if (animator.GetCurrentAnimatorClipInfoCount(0) > 0) {
                if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == AnimationTags.ENEMY_SMALL_IDLE) {
                    EnemyDie();
                }
            }
        }
    }

    void OnDestroy() {
        GameManager.Instance.currentEnemies.Remove(gameObject);    
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
            healthSystem.TakeDamage(bulletDamage);
            floatingText.InstantiateFloatingText((bulletDamage * 100).ToString(), transform);
        }

        if (collider.gameObject.CompareTag(Tags.EXPLOSION)) {
            const float EXPLOSION_DAMAGE = .25f; 
            healthSystem.TakeDamage(EXPLOSION_DAMAGE);
            floatingText.InstantiateFloatingText((EXPLOSION_DAMAGE * 100).ToString(), transform);
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
            animator.Play(AnimationTags.ENEMY_DEFAULT_IDLE);
        } else if (IsEnemyTypeRunner()) {
            animator.Play(AnimationTags.ENEMY_SMALL_IDLE);
        } else if (IsEnemyTypeGiant()) {
            animator.Play(AnimationTags.ENEMY_BIG_IDLE);
        } else if (IsEnemyTypeShooter()) {
            animator.Play(AnimationTags.ENEMY_SHOOTER_IDLE);
        } else if (IsEnemyTypeBoss()) {
            animator.Play(AnimationTags.ENEMY_BOSS_IDLE);
        }
    }

    public void PlayEnemyWalkAnimation() {
        if (healthSystem.IsDie()) return;

        if (IsEnemyTypeDefault()) {
            animator.Play(AnimationTags.ENEMY_DEFAULT_WALK);
        } else if (IsEnemyTypeRunner()) {
            animator.Play(AnimationTags.ENEMY_SMALL_WALK);
        } else if (IsEnemyTypeGiant()) {
            animator.Play(AnimationTags.ENEMY_BIG_WALK);
        } else if (IsEnemyTypeShooter()) {
            animator.Play(AnimationTags.ENEMY_SHOOTER_WALK);
        } else if (IsEnemyTypeBoss()) {
            animator.Play(AnimationTags.ENEMY_BOSS_WALK);
        }
    }

    public void PlayEnemyHurtAnimation() {
        if (healthSystem.IsDie()) return;

        if (IsEnemyTypeDefault()) {
            animator.Play(AnimationTags.ENEMY_DEFAULT_HURT);
        } else if (IsEnemyTypeRunner()) {
            animator.Play(AnimationTags.ENEMY_SMALL_HURT);
        } else if (IsEnemyTypeGiant()) {
            animator.Play(AnimationTags.ENEMY_BIG_HURT);
        } else if (IsEnemyTypeShooter()) {
            animator.Play(AnimationTags.ENEMY_SHOOTER_HURT);
        } else if (IsEnemyTypeBoss()) {
            animator.Play(AnimationTags.ENEMY_BOSS_HURT);
        }
    }

    public void PlayEnemyAttackAnimation() {
        if (healthSystem.IsDie()) return;

        if (IsEnemyTypeDefault()) {
            animator.Play(AnimationTags.ENEMY_DEFAULT_ATTACK);
        } else if (IsEnemyTypeRunner()) {
            animator.Play(AnimationTags.ENEMY_SMALL_ATTACK);
        } else if (IsEnemyTypeGiant()) {
            animator.Play(AnimationTags.ENEMY_BIG_ATTACK);
        } else if (IsEnemyTypeShooter()) {
            animator.Play(AnimationTags.ENEMY_SHOOTER_SHOOT);
        } else if (IsEnemyTypeBoss()) {
            animator.Play(AnimationTags.ENEMY_BOSS_ATTACK);
        }
    }

    private bool isAnimationDieCalled = false;
    void PlayEnemyDieAnimation() {
        if (isAnimationDieCalled) return;
        GameObject particle = Instantiate(particlePrefab, transform.position, Quaternion.identity);

        switch (enemyType) {
            case EnemyType.DEFAULT:
                SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.ENEMY_DIE_DEFAULT);
                animator.Play(AnimationTags.ENEMY_DEFAULT_DIE);
                break;
            case EnemyType.RUNNER:
                SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.ENEMY_DIE_SMALL);
                animator.Play(AnimationTags.ENEMY_SMALL_DIE);
                break;
            case EnemyType.GIANT:
                SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.ENEMY_DIE_BIG);
                animator.Play(AnimationTags.ENEMY_BIG_DIE);
                break;
            case EnemyType.SHOOTER:
                SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.ENEMY_DIE_DEFAULT);
                break;
            case EnemyType.BOSS:
                SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.ENEMY_DIE_BIG);
                animator.Play(AnimationTags.ENEMY_BOSS_DIE);
                break;  
        }

        isAnimationDieCalled = true;
    } 


    public void EnemyDie() {
        if (IsEnemyTypeShooter()) {
            GameObject particle = Instantiate(particlePrefab, transform.position, Quaternion.identity);
        }
        Instantiate(coinPrefab, transform.position, Quaternion.identity);
        CollectibleItem.Instance.AddEnemyKillCounter();
        if (CollectibleItem.Instance.canSpawnCollectibleItem) {
            GameObject collectibleItemObject = Instantiate(CollectibleItem.Instance.collectibleItemObject, transform.position + Vector3.down, Quaternion.identity);
            collectibleItemObject.GetComponent<SpriteRenderer>().sprite = CollectibleItem.Instance.currentItem.icon;
            collectibleItemObject.GetComponent<SpriteRenderer>().material = CollectibleItem.Instance.currentItem.material;
            collectibleItemObject.GetComponent<CollectibleItemObject>().item = CollectibleItem.Instance.currentItem;
        }

        MissionManager.Instance.UpdateMissionProgress(EnumsManager.Mission.NUMBER_OF_ENEMIES_KILLED);

        Destroy(gameObject);
    }
}
