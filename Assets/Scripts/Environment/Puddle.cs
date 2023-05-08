using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puddle : MonoBehaviour {
    [Header("Move Attack Settings")]
    [SerializeField] private float moveSpeedDivideBy;
    private float initialSpeed;

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.CompareTag(Tags.ENEMY) && !collider.isTrigger) {
            initialSpeed = collider.GetComponent<EnemyMovement>().maxSpeed;
            collider.GetComponent<EnemyMovement>().SetMaxSpeed(initialSpeed / moveSpeedDivideBy);
        }
    }


    void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.CompareTag(Tags.ENEMY) && !collider.isTrigger) {
            collider.GetComponent<EnemyMovement>().SetMaxSpeed(initialSpeed);
        }
    }
}
