using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornMine : MonoBehaviour {
    [SerializeField] private float damageAmount;
    [SerializeField] private float moveSpeedDivideBy;
    private float initialSpeed;
    private bool canAttack;

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.CompareTag(Tags.ENEMY)) {
            canAttack = true;
            // Make enemy slower
            initialSpeed = collider.GetComponent<EnemyMovement>().maxSpeed;
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
            // Restore enemy speed
            collider.GetComponent<EnemyMovement>().SetMaxSpeed(initialSpeed);
        }
    }

    IEnumerator AttackEnemy(GameObject gameObject, float waitForSeconds) {
        canAttack = false;
        yield return new WaitForSecondsRealtime(waitForSeconds);
        if (gameObject != null) {
            gameObject.GetComponent<Animator>().Play(AnimationTags.ENEMY_HURT);
            gameObject.GetComponent<HealthSystem>().TakeDamage(damageAmount);
        }
        canAttack = true;
    }
}
