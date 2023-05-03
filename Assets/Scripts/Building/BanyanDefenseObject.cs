using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanyanDefenseObject : MonoBehaviour {
    public EnumsManager.DefenseType defenseType;

    [SerializeField] private bool hasParticle;
    [SerializeField] private GameObject particle;

    private void Start() {
        if (hasParticle) {
            foreach (Transform child in transform) {
                Instantiate(particle, child);
            }
        }
    }
}
