using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class ExplodeRandom : MonoBehaviour {
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private int maxExplosion;

    void Start() {
        SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.PLAYER_SKILL_EXPLODE_RANDOM);
        CameraShaker.Instance.ShakeOnce(5f, 5f, 1f, 3f);

        List<GameObject> explosionTarget = new List<GameObject>();
        for (int i = 0; i < maxExplosion; i++) {
            foreach (var enemy in GameManager.Instance.currentEnemies) {
                if (!explosionTarget.Contains(enemy)) {
                    explosionTarget.Add(enemy);
                    break;
                }
            }
        }

        foreach (var e in explosionTarget) {
            if (e != null) {
                GameObject explosion = Instantiate(explosionPrefab, e.transform.position, Quaternion.identity);
                Destroy(explosion, CollectibleItem.Instance.currentItem.activateTime);
                SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.EXPLOSION);
            }
        }
    }
}
