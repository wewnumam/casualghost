using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public static LevelManager Instance { get; private set; }

    public List<Level> levels;
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private List<Transform> enemySpawners;
    [SerializeField] private Transform enemyParent;
    public LevelState[] levelStates;
    public int levelStateIndex;

    void Awake () {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        levelStates = (LevelState[])System.Enum.GetValues(typeof(LevelState));
    }

    public void StartLevel(float enemyAmount) {
        for (int i = 0; i < enemyAmount; i++) {
            StartCoroutine(SpawnEnemy(Random.Range(0, GameManager.Instance.playTimeInSeconds * 0.9f)));
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
    public int gemsObtained;

    public Level(LevelState levelState, float enemyAmount, int gemsObtained) {
        this.levelState = levelState;
        this.enemyAmount = enemyAmount;
        this.gemsObtained = gemsObtained;
    }
}

public enum LevelState {
    LEVEL_1,
    LEVEL_2,
    LEVEL_3
}
