using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapRoot : MonoBehaviour {
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite attackSprite;
    private bool canTrap = true;
    private float initialSpeed;
    [SerializeField] private float trapTime;

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.CompareTag(Tags.ENEMY) && !collider.isTrigger && canTrap) {
            if (collider.GetComponent<Enemy>().IsEnemyTypeDefault() || collider.GetComponent<Enemy>().IsEnemyTypeGiant()) {
                StartCoroutine(FreezeEnemyMovement(collider.GetComponent<EnemyMovement>()));
            }
        }
    }

    IEnumerator FreezeEnemyMovement(EnemyMovement enemyMovement) {
        yield return new WaitForSeconds(1f);
        spriteRenderer.sprite = attackSprite;
        initialSpeed = enemyMovement.maxSpeed;
        enemyMovement.SetMaxSpeed(0);
        canTrap = false;
        yield return new WaitForSeconds(trapTime);
        spriteRenderer.sprite = idleSprite;
        enemyMovement.SetMaxSpeed(initialSpeed);
    }


    void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.CompareTag(Tags.ENEMY) && !collider.isTrigger) {
            if (collider.GetComponent<Enemy>().IsEnemyTypeDefault() || collider.GetComponent<Enemy>().IsEnemyTypeGiant()) {
                canTrap = true;
            }
        }
    }
}
