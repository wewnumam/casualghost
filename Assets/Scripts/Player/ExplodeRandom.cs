using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeRandom : MonoBehaviour {
    [SerializeField] private GameObject explosionPrefab;
    private GameObject[] enemies;
    [SerializeField] private int maxExplosion;

    void Start() {
        enemies = GameObject.FindGameObjectsWithTag(Tags.ENEMY);

        List<GameObject> explosionTarget = new List<GameObject>();
        for (int i = 0; i < maxExplosion; i++) {
            foreach (var enemy in enemies) {
                if (!explosionTarget.Contains(enemy)) {
                    explosionTarget.Add(enemy);
                    break;
                }
            }
        }

        foreach (var e in explosionTarget) {
            GameObject explosion = Instantiate(explosionPrefab, e.transform.position, Quaternion.identity);
            Destroy(explosion, CollectibleItem.Instance.currentItem.activateTime);
            SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.EXPLOSION);
        }
    }
}
