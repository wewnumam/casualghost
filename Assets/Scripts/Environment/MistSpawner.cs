using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MistSpawner : MonoBehaviour {
    [Header("Mist Properties")]
    [SerializeField] private GameObject mistPrefab;
    [SerializeField] private int mistAmount;

    [Header("Spawn Position")]
    [SerializeField] private Transform north;
    [SerializeField] private Transform northEast;
    [SerializeField] private Transform east;
    [SerializeField] private Transform southEast;
    [SerializeField] private Transform south;
    [SerializeField] private Transform southWest;
    [SerializeField] private Transform west;
    [SerializeField] private Transform northWest;
    private Transform[] spawnPositions;

    void Start() {
        spawnPositions = new Transform[] {north, northEast, east, southEast, south, southWest, west, northWest};

        for (int i = 0; i < mistAmount; i++) {
            StartCoroutine(SpawnMist(Random.Range(0, GameManager.Instance.playTimeInSeconds * 0.9f)));
        }
    }

    IEnumerator SpawnMist(float waitForSeconds) {
        yield return new WaitForSeconds(waitForSeconds);
        Transform spawnPosition = spawnPositions[Random.Range(0, spawnPositions.Length)];
        GameObject mistObject = Instantiate(mistPrefab, spawnPosition);
        Mist mist = mistObject.GetComponent<Mist>();
        if (spawnPosition == north) {
            mist.isMoveDown = true;
        } else if (spawnPosition == northEast) {
            mist.isMoveDown = true;
            mist.isMoveLeft = true;
        } else if (spawnPosition == east) {
            mist.isMoveLeft = true;
        } else if (spawnPosition == southEast) {
            mist.isMoveUp = true;
            mist.isMoveLeft = true;
        } else if (spawnPosition == south) {
            mist.isMoveUp = true;
        } else if (spawnPosition == southWest) {
            mist.isMoveUp = true;
            mist.isMoveRight = true;
        } else if (spawnPosition == west) {
            mist.isMoveRight = true;
        } else if (spawnPosition == northWest) {
            mist.isMoveDown = true;
            mist.isMoveRight = true;
        }
    }
}
