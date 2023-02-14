using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private Transform player;
    private float playerDefaultMoveSpeed;
    private bool hasAttack;
    [SerializeField] private GameObject coin;

    void Start() {
        player = GameObject.FindWithTag(Tags.PLAYER).transform;
        playerDefaultMoveSpeed = player.GetComponent<PlayerMovement>().moveSpeed;
    }

    void Update() {
        FollowPlayer();
    }

    void FollowPlayer() {
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
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
        if (collision.gameObject.CompareTag(Tags.BULLET_TYPE_ONE)) {
            const int BULLET_DAMAGE = 1;
            GetComponent<HealthSystem>().TakeDamage(BULLET_DAMAGE);

            if (GetComponent<HealthSystem>().currentHealth == 0) {
                Instantiate(coin, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
