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

    void Start() {
        player = GameObject.FindWithTag(Tags.PLAYER).transform;
        playerDefaultMoveSpeed = player.GetComponent<CharacterPlayable>().moveSpeed;
    }

    void Update() {
        if (!hasAttack) FollowPlayer();
    }

    void FollowPlayer() {
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag(Tags.PLAYER)) {
            hasAttack = true;

            // Player trance behavior
            collision.gameObject.GetComponent<Animator>().Play(AnimationTags.PLAYER_TRANCE);
            collision.gameObject.GetComponent<CharacterPlayable>().moveSpeed = 0f;
            
            // Enemy attack behavior
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 0f, .5f);
            
            // Deactivate attack on delay
            float attackTime = 5f;
            Invoke("DeactivateAttack", attackTime);
        }
    }
    
    void DeactivateAttack() {
        // Set default player behavior
        player.GetComponent<Animator>().Play(AnimationTags.PLAYER_IDLE);
        player.GetComponent<CharacterPlayable>().moveSpeed = playerDefaultMoveSpeed;
        
        hasAttack = false;
        Destroy(gameObject);
    }
}
