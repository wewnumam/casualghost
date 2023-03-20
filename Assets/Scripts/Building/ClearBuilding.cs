using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearBuilding : MonoBehaviour {
    private List<string> buildingsCanDestroyed;
    private List<GameObject> objectsOnCollider;
    private bool canDestroy; // Prevent objects from being destroyed until a certain condition is met
    [SerializeField] private GameObject explosionPrefab;

    void Start() {
        // Create a new list and add tags of buildings that can be destroyed
        buildingsCanDestroyed = new List<string>();
        buildingsCanDestroyed.Add(Tags.THORN_MINE);
        buildingsCanDestroyed.Add(Tags.ROOT);
        buildingsCanDestroyed.Add(Tags.DECOY);
        buildingsCanDestroyed.Add(Tags.CANNON);

        // Create an empty list to keep track of objects on collider
        objectsOnCollider = new List<GameObject>();

        canDestroy = true;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        // If the colliding object has a specific tag, destroy all objects on the collider and the game object itself
        if (collider.gameObject.CompareTag(Tags.BULLET_TYPE_ONE) || collider.gameObject.CompareTag(Tags.BULLET_TYPE_TWO)) {
            SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.EXPLOSION);
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 2f);

            canDestroy = false;

            foreach (var o in objectsOnCollider) {
                Destroy(o);
            }
            Destroy(gameObject);
        }

        // Check if the colliding object has one of the specified building tags, and add it to the objectsOnCollider list
        foreach (var building in buildingsCanDestroyed) {
            if (collider.gameObject.CompareTag(building)) {
                objectsOnCollider.Add(collider.gameObject);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (!canDestroy) return;

        // If the colliding object has one of the specified building tags, remove it from the objectsOnCollider list
        foreach (var building in buildingsCanDestroyed) {
            if (collider.gameObject.CompareTag(building)) {
                objectsOnCollider.Remove(collider.gameObject);
            }
        }
    }
}
