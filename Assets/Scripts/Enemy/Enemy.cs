using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] private EnemyType enemyType;

    [Header("Coin Instantiate Properties")]
    [SerializeField] private GameObject coinPrefab;

    private enum EnemyType {
        THE_DEFAULT,
        THE_SPRINTER,
        THE_INVULNERABLE,
        THE_SHOOTER
    }

    void Start() {
        switch (enemyType) {
            case EnemyType.THE_DEFAULT:
                SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.ENEMY_SPAWN_DEFAULT);
                break;
            case EnemyType.THE_SPRINTER:
                SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.ENEMY_SPAWN_SMALL);
                break;
            case EnemyType.THE_INVULNERABLE:
                SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.ENEMY_SPAWN_BIG);
                break;
            case EnemyType.THE_SHOOTER:
                SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.ENEMY_SPAWN_DEFAULT);
                break;
        }
    }

    void Update() {
        if (GetComponent<HealthSystem>().IsDie()) {
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        // Enemy take damage from bullet
        if (collision.gameObject.CompareTag(Tags.BULLET_TYPE_ONE)) {
            SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.ENEMY_BLOOD);
            float bulletDamage = collision.gameObject.GetComponent<Projectile>().bulletDamage;
            GetComponent<HealthSystem>().TakeDamage(bulletDamage);
        }
    }

    void OnMouseEnter() {
        GameCursor.Instance.SetEnemyCursor();
    }

    void OnMouseExit() {
        GameCursor.Instance.SetDefaultCursor();
    }

    public bool IsEnemyTypeDefault() => enemyType == EnemyType.THE_DEFAULT;

    public void PlayEnemyIdleAnimation() {
        if (IsEnemyTypeDefault()) {
            GetComponent<Animator>().Play(AnimationTags.ENEMY_DEFAULT_IDLE);
        }
    }

    public void PlayEnemyWalkAnimation() {
        if (IsEnemyTypeDefault()) {
            GetComponent<Animator>().Play(AnimationTags.ENEMY_DEFAULT_WALK);
        }
    }

    public void PlayEnemyHurtAnimation() {
        if (IsEnemyTypeDefault()) {
            GetComponent<Animator>().Play(AnimationTags.ENEMY_DEFAULT_HURT);
        }
    }
}
