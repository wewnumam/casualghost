using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornMine : MonoBehaviour {
    [Header("Health Attack Settings")]
    [SerializeField] private float damageAmount;
    private bool canAttack;

    [Header("Move Attack Settings")]
    [SerializeField] private float moveSpeedDivideBy;
    private float initialSpeed;

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
            StartCoroutine(AttackEnemy(collider.gameObject, 1f));
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.CompareTag(Tags.ENEMY)) {
            // Restore the enemy's initial speed
            collider.GetComponent<EnemyMovement>().SetMaxSpeed(initialSpeed);
        }
    }

    IEnumerator AttackEnemy(GameObject gameObject, float waitForSeconds) {
        canAttack = false; // Prevent attacking during the delay
        yield return new WaitForSecondsRealtime(waitForSeconds);
        if (gameObject != null) {
            gameObject.GetComponent<Enemy>().PlayEnemyHurtAnimation();
            gameObject.GetComponent<HealthSystem>().TakeDamage(damageAmount);
        }
        canAttack = true; // Allow attacking again
    }
}
