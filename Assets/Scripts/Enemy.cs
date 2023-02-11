using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health = 1;
    [SerializeField] private float moveSpeed;
    private Transform player;
    private float playerDefaultMoveSpeed;
    private bool hasAttack;

    void Start() {
        player = GameObject.FindWithTag(Tags.PLAYER).transform;
        playerDefaultMoveSpeed = player.GetComponent<CharacterPlayable>().moveSpeed;
    }

    void Update() {
        FollowPlayer();
    }

    void FollowPlayer() {
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        float angle = UtilsClass.GetAngleFromVectorFloat((player.position - transform.position).normalized);
        transform.eulerAngles = new Vector3(0, 0, angle - 90f);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag(Tags.BULLET_TYPE_ONE)) {
            const int BULLET_DAMAGE = 1;
            health -= BULLET_DAMAGE;

            if (health == 0) {
                Destroy(gameObject);
            }
        }
    }
}
