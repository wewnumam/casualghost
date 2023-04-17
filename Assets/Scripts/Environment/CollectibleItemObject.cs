using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleItemObject : MonoBehaviour {
    public Item item;
    public float lifeTime;

    void Awake() {
        GameObject branch = Instantiate(new GameObject("Branch"), transform.position, Quaternion.identity);
        branch.AddComponent<SpriteRenderer>();
        branch.GetComponent<SpriteRenderer>().sprite = CollectibleItem.Instance.branchSprite;
        branch.GetComponent<SpriteRenderer>().material = CollectibleItem.Instance.branchMaterial;
        branch.GetComponent<SpriteRenderer>().sortingLayerName = "Character";
        branch.AddComponent<PositionRendererSorter>();
        branch.GetComponent<PositionRendererSorter>().offset = 2;
        branch.GetComponent<PositionRendererSorter>().runOnlyOnce = true;
        Destroy(branch, lifeTime);
        Destroy(gameObject, lifeTime);    
    }
}
