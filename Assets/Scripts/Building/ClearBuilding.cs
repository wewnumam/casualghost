using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearBuilding : MonoBehaviour
{
    private List<string> buildingsCanDestroyed;
    private List<GameObject> objectsOnCollider;
    private bool canDestroy;

    void Start() {
        buildingsCanDestroyed = new List<string>();
        buildingsCanDestroyed.Add(Tags.THORN_MINE);
        buildingsCanDestroyed.Add(Tags.ROOT);

        objectsOnCollider = new List<GameObject>();
    }

    void Update() {
        if (canDestroy) {
            foreach (var o in objectsOnCollider) {
                Destroy(o);
            }
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.CompareTag(Tags.BULLET_TYPE_ONE)) {
            canDestroy = true;
        }

        foreach (var building in buildingsCanDestroyed) {
            if (collider.gameObject.CompareTag(building)) {
                objectsOnCollider.Add(collider.gameObject);
            }
        }
    }
}
