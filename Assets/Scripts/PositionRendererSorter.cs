using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode()]
public class PositionRendererSorter : MonoBehaviour {
    [SerializeField] private int sortingOrderBase;
    [SerializeField] private int offset;
    [SerializeField] private bool runOnlyOnce;
    private SpriteRenderer spriteRenderer;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void LateUpdate() {
        spriteRenderer.sortingOrder = (int)(sortingOrderBase - transform.position.y - offset);
        if (runOnlyOnce) {
            DestroyImmediate(this);
        }     
    }
}
