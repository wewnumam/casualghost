using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour {
    public float maxHealth;
    [HideInInspector] public float currentHealth;

    void Awake() {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount) {
        if (currentHealth > 0f) {
            currentHealth -= amount;
        } else {
            currentHealth = 0f;
        }
    }
}
