using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public static LevelManager Instance { get; private set; }

    public List<Level> levels;
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private List<Transform> enemySpawners;
    [SerializeField] private Transform enemyParent;

    void Awake () {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void StartLevel(float enemyAmount) {
        for (int i = 0; i < enemyAmount; i++) {
            StartCoroutine(SpawnEnemy(Random.Range(0, GameManager.Instance.playTimeInSeconds)));
        }
    }

    IEnumerator SpawnEnemy(float waitForSeconds) {
        yield return new WaitForSeconds(waitForSeconds);

        Instantiate(enemies[Random.Range(0, enemies.Count)],
                    enemySpawners[Random.Range(0, enemySpawners.Count)].position,
                    Quaternion.identity,
                    enemyParent);
    }
}

[System.Serializable]
public class Level {
    public LevelState levelState;
    public float enemyAmount;

    public Level(LevelState levelState, float enemyAmount) {
        this.levelState = levelState;
        this.enemyAmount = enemyAmount;
    }
}

public enum LevelState {
    LEVEL_1,
    LEVEL_2
}
