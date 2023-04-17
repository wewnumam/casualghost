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

    void Update() {
        if (isMoveLeft) {
            GetComponent<Rigidbody2D>().AddForce(Vector2.left * _maxSpeed);
        }

        if (isMoveRight) {
            GetComponent<Rigidbody2D>().AddForce(Vector2.right * _maxSpeed);
        }

        if (isMoveUp) {
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * _maxSpeed);
        }

        if (isMoveDown) {
            GetComponent<Rigidbody2D>().AddForce(Vector2.down * _maxSpeed);
        }

        // Limit the velocity to a maximum value
        float currentSpeed = GetComponent<Rigidbody2D>().velocity.magnitude;
        if (currentSpeed > _maxSpeed) {
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity.normalized * _maxSpeed;
        }
    }
}
