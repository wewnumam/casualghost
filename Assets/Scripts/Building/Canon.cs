using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour {
    [Header("Shooting Properties")]
	[SerializeField] private float pullTriggerTime;
	private bool canShoot = true;

    [Header("Bullet Instantiate Properties")]
    [SerializeField] private GameObject bulletPrefab;
	[SerializeField] private Transform bulletSpawnPoint;

    private GameObject enemy;

    void Update() {
        // If the canon is able to shoot and there is at least one enemy
        if (canShoot && enemy != null) {

            // Rotate the canon's transform to aim at the closest enemy
            UtilsClass.AimRotation(transform, enemy.transform.position);

            // Play the shooting sound effect
            SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.CANON_SHOOT);

            // Instantiate a bullet prefab at the bullet spawn point
            GameObject b = Instantiate(
                bulletPrefab,
                bulletSpawnPoint.position,
                bulletSpawnPoint.rotation
            );

            // Draw LineRenderer
            b.GetComponentInChildren<Projectile>().isDrawingLine = true;

            // The canon can't shoot again until a certain amount of time has passed
            canShoot = false;

            // Start a coroutine to delay the shooting cooldown
            StartCoroutine(PullTrigger(pullTriggerTime));
        }
    }

    // Delays the shooting cooldown, waiting for a certain period of time
	private IEnumerator PullTrigger(float time) {
        yield return new WaitForSeconds(time);
        canShoot = true;
    }

    void OnTriggerStay2D(Collider2D collider) {
        if (collider.gameObject.CompareTag(Tags.ENEMY)) {
            enemy = collider.gameObject;
        }
    }
}
