using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mist : MonoBehaviour {
    [SerializeField] private float _maxSpeed;

    void Update() {
        GetComponent<Rigidbody2D>().AddForce(Vector2.left * _maxSpeed);

        // Limit the velocity to a maximum value
        float currentSpeed = GetComponent<Rigidbody2D>().velocity.magnitude;
        if (currentSpeed > _maxSpeed) {
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity.normalized * _maxSpeed;
        }
    }
}
