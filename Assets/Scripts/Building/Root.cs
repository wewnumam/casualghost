using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour {
    [SerializeField] private RootSpriteRenderer[] rootSpriteRenderers;

    void Awake() {
        RootSpriteRenderer root = rootSpriteRenderers[Random.Range(0, rootSpriteRenderers.Length)];
        GetComponent<SpriteRenderer>().sprite = root.rootSprites;
        GetComponent<SpriteRenderer>().material = root.rootMaterial;
    }
}

[System.Serializable]
public class RootSpriteRenderer {
    public Sprite rootSprites;
    public Material rootMaterial;
}
