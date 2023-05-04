using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mist : MonoBehaviour {
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _damageAmount;
    public float damageAmount { get => _damageAmount; }
    [SerializeField] private float _attackSpeed;
    public float attackSpeed { get => _attackSpeed; }
    
    public bool isMoveLeft;
    public bool isMoveRight;
    public bool isMoveUp;
    public bool isMoveDown;

    [Header("Caching Components")]
    [SerializeField] private new Rigidbody2D rigidbody2D;

    void Update() {
        if (isMoveLeft) {
            rigidbody2D.AddForce(Vector2.left * _maxSpeed);
        }

        if (isMoveRight) {
            rigidbody2D.AddForce(Vector2.right * _maxSpeed);
        }

        if (isMoveUp) {
            rigidbody2D.AddForce(Vector2.up * _maxSpeed);
        }

        if (isMoveDown) {
            rigidbody2D.AddForce(Vector2.down * _maxSpeed);
        }

        // Limit the velocity to a maximum value
        float currentSpeed = rigidbody2D.velocity.magnitude;
        if (currentSpeed > _maxSpeed) {
            rigidbody2D.velocity = rigidbody2D.velocity.normalized * _maxSpeed;
        }
    }
}
