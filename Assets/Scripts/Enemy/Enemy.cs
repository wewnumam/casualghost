using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public EnemyType enemyType;
    public float maxSpeed;
    private Transform[] targetToFollow;
    [SerializeField] private GameObject coinPrefab;

    void Update() {
        GameObject[] decoys = GameObject.FindGameObjectsWithTag(Tags.DECOY);

        // Prioritize to follow decoy
        if (decoys.Length > 0) {
            Transform[] decoyToFollow = new Transform[decoys.Length];

            for (int i = 0; i < decoys.Length; i++) {
                decoyToFollow[i] = decoys[i].transform;
            }

            FollowTarget(UtilsClass.FindClosestTransform(this.transform, decoyToFollow));
        } else {
            GameObject[] canons = GameObject.FindGameObjectsWithTag(Tags.CANON);
            
            // Check if any canons were found 
            if (canons.Length > 0) {
                targetToFollow = new Transform[canons.Length + 2];
                for (int i = 0; i < canons.Length; i++) {
                    targetToFollow[i] = canons[i].transform;
                }
                targetToFollow[canons.Length] = GameObject.FindWithTag(Tags.PLAYER).transform;
                targetToFollow[canons.Length + 1] = GameObject.FindWithTag(Tags.BANYAN).transform;
            } else {
                targetToFollow = new Transform[] {
                    GameObject.FindWithTag(Tags.PLAYER).transform,
                    GameObject.FindWithTag(Tags.BANYAN).transform
                };
            }

            FollowTarget(UtilsClass.FindClosestTransform(this.transform, targetToFollow));
        }

        // Enemy health check
        if (GetComponent<HealthSystem>().currentHealth <= 0) {
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void FollowTarget(Transform target) {
        // Enemy follow target
        // transform.position = Vector2.MoveTowards(transform.position, target.position, maxSpeed * Time.deltaTime);
        Vector3 moveDir = (target.position - transform.position).normalized;
        GetComponent<Rigidbody2D>().AddForce(moveDir * maxSpeed);

        // Limit the velocity to a maximum value
        float currentSpeed = GetComponent<Rigidbody2D>().velocity.magnitude;
        if (currentSpeed > maxSpeed) {
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity.normalized * maxSpeed;
        }

        // Enemy look at target
        float angle = UtilsClass.GetAngleFromVectorFloat((target.position - transform.position).normalized);
        Vector3 localScale = Vector3.one;
		if (angle > 90 && angle < 270) {
			localScale.x *= -1f;
		} else {
			localScale.x *= +1f;
		}
		transform.localScale = localScale;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        // Enemy take damage from bullet
        if (collision.gameObject.CompareTag(Tags.BULLET_TYPE_ONE)) {
            float bulletDamage = collision.gameObject.GetComponent<Projectile>().bulletDamage;
            GetComponent<HealthSystem>().TakeDamage(bulletDamage);
        }
    }
}

public enum EnemyType {
	THE_DEFAULT,
	THE_SPRINTER,
    THE_INVULNERABLE,
    THE_SHOOTER
}