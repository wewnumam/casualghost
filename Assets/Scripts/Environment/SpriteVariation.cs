using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteVariation : MonoBehaviour {
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;

    void Awake() {
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
    }
}
