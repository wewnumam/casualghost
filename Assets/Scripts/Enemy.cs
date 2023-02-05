using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health = 5;
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
        transform.LookAt(player.position, Vector3.forward);
        transform.eulerAngles = new Vector3(0, 0, -transform.eulerAngles.z);
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
        
        else if (collision.gameObject.CompareTag(Tags.BULLET_LV1_ALIF)) {
            const int BULLET_DAMAGE = 1;
            health -= BULLET_DAMAGE;

            if (health == 0) {
                Destroy(gameObject);
            }
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
