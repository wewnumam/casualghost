using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricades : MonoBehaviour {
    [SerializeField] private float _maxSpeed;
    
    public bool isMoveLeft;
    public bool isMoveRight;
    public bool isMoveUp;
    public bool isMoveDown;

    private Rigidbody2D[] rigidbody2Ds;

    void Update() {
        rigidbody2Ds = GetComponentsInChildren<Rigidbody2D>();
        
        if (isMoveLeft) {
            AddForce(Vector2.left * _maxSpeed);
        }

        if (isMoveRight) {
            AddForce(Vector2.right * _maxSpeed);
        }

        if (isMoveUp) {
            AddForce(Vector2.up * _maxSpeed);
        }

        if (isMoveDown) {
            AddForce(Vector2.down * _maxSpeed);
        }

        // Limit the velocity to a maximum value
        for (int i = 0; i < transform.childCount; i++) {    
            float currentSpeed = rigidbody2Ds[i].velocity.magnitude;
            if (currentSpeed > _maxSpeed && rigidbody2Ds[i] != null) {
                rigidbody2Ds[i].velocity = rigidbody2Ds[i].velocity.normalized * _maxSpeed;
            }
        }
    }

    void AddForce(Vector2 force) {
        for (int i = 0; i < transform.childCount; i++) {
            if (rigidbody2Ds[i] != null && rigidbody2Ds[i].GetComponent<HealthSystem>().IsAlive()) {
                rigidbody2Ds[i].AddForce(force);
            }
        }
    }
}
