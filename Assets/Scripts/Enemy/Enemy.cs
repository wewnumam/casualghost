using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public float maxSpeed;
    private Transform[] targetToFollow;
    [SerializeField] private GameObject coinPrefab;

    void Start() {
        targetToFollow = new Transform[] {
            GameObject.FindWithTag(Tags.PLAYER).transform,
            GameObject.FindWithTag(Tags.BANYAN).transform
        };
    }

    void Update() {
        FollowTarget(FindClosestTransform(targetToFollow));

        // Enemy health check
        if (GetComponent<HealthSystem>().currentHealth <= 0) {
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    Transform FindClosestTransform(Transform[] transforms) {
        Transform closestTransform = null;
        float closestDistance = Mathf.Infinity;
    
        foreach (Transform t in transforms) 
        {
            float distanceToTarget = Vector3.Distance(t.position, transform.position);
            if (distanceToTarget < closestDistance) {
                closestTransform = t;
                closestDistance = distanceToTarget;
            }
        }
    
        return closestTransform;
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
            const float BULLET_DAMAGE = 1f;
            GetComponent<HealthSystem>().TakeDamage(BULLET_DAMAGE);
        }
    }
}
