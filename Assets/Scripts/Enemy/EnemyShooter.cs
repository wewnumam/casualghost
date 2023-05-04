using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour {
    [Header("Caching Components")]
    [SerializeField] private Enemy enemy;
    [SerializeField] private EnemyMovement enemyMovement;

    [Header("Shooting Properties")]
	[SerializeField] private float pullTriggerTime = 2f;
	private bool canShoot;

    [Header("Bullet Instantiate Properties")]
    [SerializeField] private GameObject bulletPrefab;
	[SerializeField] private Transform bulletSpawnPoint;
    private float initialMaxSpeed;
    
    void Awake() {
        canShoot = true;
        initialMaxSpeed = enemyMovement.maxSpeed;
    }

    void Update() {
        // Store GameObjects in the scene with the tag "PLAYER" and "BANYAN" in a Transform array
        Transform[] targetToShoot = new Transform[] {
            Player.Instance.transform,
            Player.Instance.transform
        };

        if (canShoot) {
            // Find the closest target to the enemy and get its transform
            Transform targetToAim = UtilsClass.FindClosestTransform(this.transform, targetToShoot);

            // Rotate the enemy's transform to aim at the closest target
            UtilsClass.AimRotation(transform, targetToAim.position + new Vector3(0, -2, 0));

             // Play the shooting sound effect
            SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.ENEMY_SHOOT);

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
        enemy.animator.Play(AnimationTags.ENEMY_SHOOTER_SHOOT);
        enemyMovement.SetMaxSpeed(0);
        yield return new WaitForSeconds(time);
        canShoot = true;
        yield return new WaitForSeconds(time / 2);
        if (enemy.healthSystem.IsAlive()) {
            enemy.animator.Play(AnimationTags.ENEMY_SHOOTER_WALK);
        } else {
            enemy.animator.Play(AnimationTags.ENEMY_SHOOTER_DIE);
        }
        enemyMovement.SetMaxSpeed(initialMaxSpeed);
    }
}
