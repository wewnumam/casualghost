using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour {
    [SerializeField] private Sprite[] treeSprites;

    void Awake() {
        GetComponent<SpriteRenderer>().sprite = treeSprites[Random.Range(0, treeSprites.Length)];
    }
}
