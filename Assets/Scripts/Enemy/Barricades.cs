using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricades : MonoBehaviour {
    [SerializeField] private float _maxSpeed;
    
    public bool isMoveLeft;
    public bool isMoveRight;
    public bool isMoveUp;
    public bool isMoveDown;

    [SerializeField] private List<Rigidbody2D> rigidbody2Ds;

    void Update() {
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
    }

    void AddForce(Vector2 force) {
        foreach (Rigidbody2D rb in rigidbody2Ds) {
            if (rb != null) {
                if (rb.GetComponent<HealthSystem>().IsAlive()) {
                    rb.AddForce(force);

                    // Limit the velocity to a maximum value
                    float currentSpeed = rb.velocity.magnitude;
                    if (currentSpeed > _maxSpeed) {
                        rb.velocity = rb.velocity.normalized * _maxSpeed;
                    }
                }
            }
        }
    }
}
