using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour {
    [SerializeField] private float _maxHealth;
    private float _currentHealth;
    public float maxHealth { get => _maxHealth; }
    public float currentHealth { get => _currentHealth; }

    void Awake() {
        _currentHealth = _maxHealth;
    }

    public void SetMaxHealth(float maxHealth) => _maxHealth = maxHealth;
    public void SetCurrentHealth(float currentHealth) => _currentHealth = currentHealth;

    public bool IsAlive() => _currentHealth > 0f;
    public bool IsDie() => _currentHealth <= 0f;

    public void TakeDamage(float damageAmount) {
        if (IsAlive()) {
            _currentHealth -= damageAmount;
        } else {
            _currentHealth = 0f;
        }
    }

    public void Heal(float healAmount) {
        _currentHealth += healAmount;

        if (_currentHealth > _maxHealth) {
            _currentHealth = _maxHealth;
        }
    }
}
