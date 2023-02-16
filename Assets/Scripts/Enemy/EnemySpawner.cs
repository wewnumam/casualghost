using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform enemyParent;
    [SerializeField] private float spawnDelayTime;

    void Start() {
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy() {
        yield return new WaitForSecondsRealtime(spawnDelayTime);
        Instantiate(enemyPrefab, transform.position, Quaternion.identity, enemyParent);

        StartCoroutine(SpawnEnemy());
    }
}
