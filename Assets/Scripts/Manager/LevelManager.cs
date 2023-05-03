using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public static LevelManager Instance { get; private set; }

    [Header("Level Properties")]
    public List<Level> levels;
    [HideInInspector] public EnumsManager.LevelState[] levelStates;
    [HideInInspector] public int levelStateIndex;
    public List<LevelAdjusment> levelAdjusmentsByWin;

    [Header("Enemy Porperties")]
    [SerializeField] private Transform enemyParent;
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private List<Transform> enemySpawners;
    private List<IEnumerator> spawnEnemyCoroutine;
    private IEnumerator currentRoutine;

    void Awake () {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        levelStates = (EnumsManager.LevelState[])System.Enum.GetValues(typeof(EnumsManager.LevelState));
    }

    public void StartLevel(float enemyAmount) {
        enemyAmount += enemyAmount * GetCurrentLevelAdjustment().increaseEnemyInPercentage / 100;
        spawnEnemyCoroutine = new List<IEnumerator>();
        for (int i = 0; i < enemyAmount; i++) {
            spawnEnemyCoroutine.Add(SpawnEnemy(Random.Range(10, GameManager.Instance.playTimeInSeconds * 0.8f)));
        }

        foreach (var c in spawnEnemyCoroutine) {
            StartCoroutine(c);
        }
    }

    public void StopSpawnEnemy() {
        foreach (var c in spawnEnemyCoroutine) {
            StopCoroutine(c);
        }

        spawnEnemyCoroutine.Clear();
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

    public LevelAdjusment GetCurrentLevelAdjustment() {
        if (PlayerPrefs.GetInt(PlayerPrefsKeys.WIN_COUNTER) >= levelAdjusmentsByWin.Count - 1) {
            return levelAdjusmentsByWin[levelAdjusmentsByWin.Count - 1];
        }
        return levelAdjusmentsByWin[PlayerPrefs.GetInt(PlayerPrefsKeys.WIN_COUNTER)];
    }
}

[System.Serializable]
public class Level {
    public EnumsManager.LevelState levelState;
    public float playTimeInSeconds;
    public float enemyAmount;
    public int gemsObtained;
    public List<int> spawnPrecentageByEnemyType;
    [Range(0, 1)] public float directionalLightIntensity;
    public Color directionalLightColor;
    public GameObject environment;
    public float cameraOrthoSize = 18;
}

[System.Serializable]
public class LevelAdjusment {
    public int increaseEnemyInPercentage;
    public int enemyKillAmountToSpawnCollectibleItem;
    public int increaseBuildCostPercentage;
}