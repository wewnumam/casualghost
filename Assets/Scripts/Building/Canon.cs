using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour {
    [SerializeField] private GameObject bulletPrefab;
	[SerializeField] private Transform bulletSpawnPoint;
	[SerializeField] private float pullTriggerTime = 1f;
	private bool canShoot;

    void Start() {
        canShoot = true;
    }

    void Update() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tags.ENEMY);

        if (canShoot && enemies.Length > 0) {
            Transform[] enemiesTransform = UtilsClass.GetGameObjectsTransform(enemies);
            Transform enemyToAim = UtilsClass.FindClosestTransform(this.transform, enemiesTransform);
            UtilsClass.AimRotation(transform, enemyToAim.position);

            SoundManager.Instance.PlayShootSFX();

            GameObject b = Instantiate(
                bulletPrefab,
                bulletSpawnPoint.position,
                bulletSpawnPoint.rotation
            );

            canShoot = false;
			StartCoroutine(PullTrigger(pullTriggerTime));
        }
    }

	private IEnumerator PullTrigger(float time) {
        yield return new WaitForSeconds(time);
        canShoot = true;
    }
}
