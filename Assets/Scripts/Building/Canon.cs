using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour {
    [SerializeField] private GameObject[] bulletTypes;
	[SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private WeaponType currentWeaponType = WeaponType.DEFAULT;
	[SerializeField] private float pullTriggerTime = 1f;
	private bool canShoot;

    void Start() {
        canShoot = true;
    }

    void Update() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tags.ENEMY);

        if (canShoot && enemies.Length > 0) {
            Transform[] enemiesTransform = new Transform[enemies.Length];
            for (int i = 0; i < enemies.Length; i++) {
                enemiesTransform[i] = enemies[i].transform;
            }
            Transform enemyToAim = UtilsClass.FindClosestTransform(this.transform, enemiesTransform);
            AimRotation(enemyToAim);

            SoundManager.Instance.PlayShootSFX();

            GameObject b = Instantiate(
                bulletTypes[(int)currentWeaponType],
                bulletSpawnPoint.position,
                bulletSpawnPoint.rotation
            );

            canShoot = false;
			StartCoroutine(PullTrigger(pullTriggerTime));
        }
    }

    void AimRotation(Transform transformToAim) {
		// Rotation facing towards mouse cursor
		Vector3 aimDirection = (transformToAim.position - transform.position).normalized;
		float angle = UtilsClass.GetAngleFromVectorFloat(aimDirection);
		transform.eulerAngles = new Vector3(0, 0, angle);

		// Flip weapon vertically
		Vector3 localScale = transform.localScale;
		if (angle > 90 && angle < 270) {
			localScale.y = -1f;
		} else {
			localScale.y = +1f;
		}
		transform.localScale = localScale;
	}

	private IEnumerator PullTrigger(float time) {
        yield return new WaitForSeconds(time);
        canShoot = true;
    }
}
