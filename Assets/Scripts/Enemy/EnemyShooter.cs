using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour {
    [Header("Shooting Properties")]
	[SerializeField] private float pullTriggerTime = 2f;
	private bool canShoot;

    [Header("Bullet Instantiate Properties")]
    [SerializeField] private GameObject bulletPrefab;
	[SerializeField] private Transform bulletSpawnPoint;
    
    void Start() {
        canShoot = true;
    }

    void Update() {
        // Store GameObjects in the scene with the tag "PLAYER" and "BANYAN" in a Transform array
        Transform[] targetToShoot = new Transform[] {
            GameObject.FindGameObjectWithTag(Tags.PLAYER).transform,
            GameObject.FindGameObjectWithTag(Tags.BANYAN).transform
        };

        if (canShoot) {
            // Find the closest target to the enemy and get its transform
            Transform targetToAim = UtilsClass.FindClosestTransform(this.transform, targetToShoot);

            // Rotate the enemy's transform to aim at the closest target
            UtilsClass.AimRotation(transform, targetToAim.position);

             // Play the shooting sound effect
            SoundManager.Instance.PlayShootSFX();

            // Instantiate a bullet prefab at the bullet spawn point
            GameObject b = Instantiate(
                bulletPrefab,
                bulletSpawnPoint.position,
                bulletSpawnPoint.rotation
            );

            // The enemy can't shoot again until a certain amount of time has passed
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
