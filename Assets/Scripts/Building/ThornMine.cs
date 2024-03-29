using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornMine : MonoBehaviour {
    [Header("Caching Components")]
    [SerializeField] private Animator animator;
    private Enemy enemy;

    [Header("Health Attack Settings")]
    [SerializeField] private float damageAmount;
    [SerializeField] private float attackSpeed;
    private bool canAttack;

    [Header("Move Attack Settings")]
    [SerializeField] private float moveSpeedDivideBy;
    private float initialSpeed;

    void Awake() {
        GameManager.Instance.currentThornMine.Add(gameObject);    
    }

    void OnDestroy() {
        GameManager.Instance.currentThornMine.Remove(gameObject);  
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.CompareTag(Tags.ENEMY)) {
            canAttack = true;
            initialSpeed = collider.GetComponent<EnemyMovement>().maxSpeed;
            // Make enemy slower
            collider.GetComponent<EnemyMovement>().SetMaxSpeed(collider.GetComponent<EnemyMovement>().maxSpeed / moveSpeedDivideBy); 
        }
    }

    void OnTriggerStay2D(Collider2D collider) {
        // Attack enemy
        if (collider.gameObject.CompareTag(Tags.ENEMY) && canAttack) {
            
            StartCoroutine(AttackEnemy(collider.gameObject, attackSpeed));
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.CompareTag(Tags.ENEMY)) {
            // Restore the enemy's initial speed
            collider.GetComponent<EnemyMovement>().SetMaxSpeed(initialSpeed);
            animator.Play(AnimationTags.THORN_MINE_IDLE);
        }
    }

    IEnumerator AttackEnemy(GameObject gameObject, float waitForSeconds) {
        enemy = gameObject.GetComponent<Enemy>();
        canAttack = false; // Prevent attacking during the delay
        yield return new WaitForSecondsRealtime(waitForSeconds);
        if (gameObject != null) {
            enemy.PlayEnemyHurtAnimation();
            enemy.healthSystem.TakeDamage(damageAmount);
            enemy.floatingText.InstantiateFloatingText((damageAmount * 100).ToString(), gameObject.transform);
            animator.Play(AnimationTags.THORN_MINE_ATTACK);
        }
        canAttack = true; // Allow attacking again
    }
}
