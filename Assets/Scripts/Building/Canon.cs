using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour {
    [Header("Shooting Properties")]
	[SerializeField] private float pullTriggerTime = 1f;
	private bool canShoot;

    [Header("Bullet Instantiate Properties")]
    [SerializeField] private GameObject bulletPrefab;
	[SerializeField] private Transform bulletSpawnPoint;

    void Start() {
        canShoot = true;
    }

    void Update() {
        // Find all GameObjects in the scene with the tag "ENEMY" and store them in an array
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tags.ENEMY);

        // If the canon is able to shoot and there is at least one enemy
        if (canShoot && enemies.Length > 0) {

            // Get the transform of all the enemy GameObjects
            Transform[] enemiesTransform = UtilsClass.GetGameObjectsTransform(enemies);

            // Find the closest enemy to the canon and get its transform
            Transform enemyToAim = UtilsClass.FindClosestTransform(this.transform, enemiesTransform);

            // Rotate the canon's transform to aim at the closest enemy
            UtilsClass.AimRotation(transform, enemyToAim.position);

            // Play the shooting sound effect
            SoundManager.Instance.PlayShootSFX();

            // Instantiate a bullet prefab at the bullet spawn point
            GameObject b = Instantiate(
                bulletPrefab,
                bulletSpawnPoint.position,
                bulletSpawnPoint.rotation
            );

            // Draw LineRenderer
            b.GetComponent<Projectile>().isDrawingLine = true;

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
}
