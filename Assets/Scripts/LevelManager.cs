using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public static LevelManager Instance { get; private set; }

    [Header("Level Properties")]
    public List<Level> levels;
    [HideInInspector] public EnumsManager.LevelState[] levelStates;
    [HideInInspector] public int levelStateIndex;

    [Header("Enemy Porperties")]
    [SerializeField] private Transform enemyParent;
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private List<Transform> enemySpawners;

    void Awake () {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        levelStates = (EnumsManager.LevelState[])System.Enum.GetValues(typeof(EnumsManager.LevelState));
    }

    public void StartLevel(float enemyAmount) {
        for (int i = 0; i < enemyAmount; i++) {
            StartCoroutine(SpawnEnemy(Random.Range(0, GameManager.Instance.playTimeInSeconds * 0.9f)));
        }
    }

    IEnumerator SpawnEnemy(float waitForSeconds) {
        yield return new WaitForSeconds(waitForSeconds);

        int randomNum = UnityEngine.Random.Range(0, 100);
        int cumulativePercentage = 0;

        for (int i = 0; i < levels[levelStateIndex].spawnPrecentageByEnemyType.Count; i++) {
            cumulativePercentage += levels[levelStateIndex].spawnPrecentageByEnemyType[i];
            if (randomNum < cumulativePercentage) {
                Instantiate(enemies[i],
                            enemySpawners[Random.Range(0, enemySpawners.Count)].position,
                            Quaternion.identity,
                            enemyParent);
                break;
            }
        }
    }

    public bool IsLastLevel() => levelStateIndex >= levelStates.Length - 1;
}

[System.Serializable]
public class Level {
    public EnumsManager.LevelState levelState;
    public float enemyAmount;
    public int gemsObtained;
    public List<int> spawnPrecentageByEnemyType;
    [Range(0, 1)] public float directionalLightIntensity;
    public Color directionalLightColor;

    public Level(EnumsManager.LevelState levelState, float enemyAmount, int gemsObtained, List<int> spawnPrecentageByEnemyType, float directionalLightIntensity) {
        this.levelState = levelState;
        this.enemyAmount = enemyAmount;
        this.gemsObtained = gemsObtained;
        this.spawnPrecentageByEnemyType = spawnPrecentageByEnemyType;
        this.directionalLightIntensity = directionalLightIntensity;
    }
}