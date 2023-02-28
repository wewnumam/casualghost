using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private GameObject coinPrefab;

    private enum EnemyType {
        THE_DEFAULT,
        THE_SPRINTER,
        THE_INVULNERABLE,
        THE_SHOOTER
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
            float bulletDamage = collision.gameObject.GetComponent<Projectile>().bulletDamage;
            GetComponent<HealthSystem>().TakeDamage(bulletDamage);
        }
    }
}
