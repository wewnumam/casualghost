using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    [SerializeField] private Slider slider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;

    void Awake() {
        GetComponent<Canvas>().worldCamera = Camera.main;

        if (HasHealthSystem()) {
            SetMaxHealth(GetComponentInParent<HealthSystem>().maxHealth);
        } else {
            Destroy(gameObject);
        }
    }

    void Update() {
        SetMaxHealth(GetComponentInParent<HealthSystem>().maxHealth);
        SetHealth(GetComponentInParent<HealthSystem>().currentHealth);
        transform.position = GetComponentInParent<Transform>().position;
    }

    bool HasHealthSystem() => GetComponentInParent<HealthSystem>() != null;
    
    public void SetMaxHealth(float maxHealth) {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(float health) {
        slider.value = health;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
