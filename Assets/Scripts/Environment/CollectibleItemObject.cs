using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleItemObject : MonoBehaviour {
    public Item item;
    public float lifeTime;

    void Awake() {
        GameObject branch = Instantiate(new GameObject("Branch"), transform.position, Quaternion.identity);
        branch.AddComponent<SpriteRenderer>();
        SpriteRenderer spriteRenderer = branch.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = CollectibleItem.Instance.branchSprite;
        spriteRenderer.material = CollectibleItem.Instance.branchMaterial;
        spriteRenderer.sortingLayerName = "Character";
        branch.AddComponent<PositionRendererSorter>();
        PositionRendererSorter positionRendererSorter = branch.GetComponent<PositionRendererSorter>();
        positionRendererSorter.offset = 2;
        positionRendererSorter.runOnlyOnce = true;
        Destroy(branch, lifeTime);
        Destroy(gameObject, lifeTime);    
    }
}
