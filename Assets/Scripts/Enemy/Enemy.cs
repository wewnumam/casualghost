using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private Transform player;
    [SerializeField] private GameObject coinPrefab;

    void Start() {
        player = GameObject.FindWithTag(Tags.PLAYER).transform;
    }

    void Update() {
        FollowPlayer();
    }

    void FollowPlayer() {
        // Enemy follow player
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        // Enemy look at player
        float angle = UtilsClass.GetAngleFromVectorFloat((player.position - transform.position).normalized);
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
            const int BULLET_DAMAGE = 1;
            GetComponent<HealthSystem>().TakeDamage(BULLET_DAMAGE);

            // Enemy health check
            if (GetComponent<HealthSystem>().currentHealth == 0) {
                Instantiate(coinPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
