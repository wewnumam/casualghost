using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] private EnemyType enemyType;

    [Header("Coin Instantiate Properties")]
    [SerializeField] private GameObject coinPrefab;

    private enum EnemyType {
        DEFAULT,
        RUNNER,
        GIANT,
        SHOOTER
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
        }
    }

    void Update() {
        if (GetComponent<HealthSystem>().IsDie()) {
           PlayEnemyDieAnimation();
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        // Enemy take damage from bullet
        if (collision.gameObject.CompareTag(Tags.BULLET_TYPE_ONE) || collision.gameObject.CompareTag(Tags.BULLET_TYPE_TWO)) {
            SoundManager.Instance.PlaySound(UtilsClass.SuffleSFX(new EnumsManager.SoundEffect[] {
                EnumsManager.SoundEffect.ENEMY_BLOOD_1,
                EnumsManager.SoundEffect.ENEMY_BLOOD_2,
                EnumsManager.SoundEffect.ENEMY_BLOOD_3
            }));
            float bulletDamage = collision.gameObject.GetComponent<Projectile>().bulletDamage;
            GetComponent<HealthSystem>().TakeDamage(bulletDamage);
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        // Enemy take damage from bullet
        if (collider.gameObject.CompareTag(Tags.BULLET_TYPE_TWO)) {
            SoundManager.Instance.PlaySound(UtilsClass.SuffleSFX(new EnumsManager.SoundEffect[] {
                EnumsManager.SoundEffect.ENEMY_BLOOD_1,
                EnumsManager.SoundEffect.ENEMY_BLOOD_2,
                EnumsManager.SoundEffect.ENEMY_BLOOD_3
            }));
            float bulletDamage = collider.gameObject.GetComponent<Projectile>().bulletDamage;
            GetComponent<HealthSystem>().TakeDamage(bulletDamage);
        }
    }

    void OnMouseEnter() {
        GameCursor.Instance.SetEnemyCursor();
    }

    void OnMouseExit() {
        GameCursor.Instance.SetDefaultCursor();
    }

    public bool IsEnemyTypeDefault() => enemyType == EnemyType.DEFAULT;

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

        if (enemyType == EnemyType.DEFAULT) {
            GetComponent<Animator>().Play(AnimationTags.ENEMY_DEFAULT_DIE);
        } else {
            EnemyDie();
        }

        isAnimationDieCalled = true;
    } 


    public void EnemyDie() {
        Instantiate(coinPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
