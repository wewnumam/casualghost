using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleItemObject : MonoBehaviour {
    public Item item;
    public float lifeTime;

    void Awake() {
        Destroy(gameObject, lifeTime);    
    }
}
