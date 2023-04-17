using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifetime : MonoBehaviour {
    [SerializeField] private float lifetime;

    void Awake() {
        Destroy(gameObject, lifetime);
    }
}
