using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode()]
public class PositionRendererSorter : MonoBehaviour {
    public int sortingOrderBase;
    public int offset;
    public bool runOnlyOnce;
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
