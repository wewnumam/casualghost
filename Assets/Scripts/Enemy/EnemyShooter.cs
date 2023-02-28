using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour {
    [SerializeField] private GameObject bulletPrefab;
	[SerializeField] private Transform bulletSpawnPoint;
	[SerializeField] private float pullTriggerTime = 2f;
	private bool canShoot;

    void Start() {
        canShoot = true;
    }

    void Update() {
        Transform[] targetToShoot = new Transform[] {
            GameObject.FindGameObjectWithTag(Tags.PLAYER).transform,
            GameObject.FindGameObjectWithTag(Tags.BANYAN).transform
        };

        if (canShoot) {
            Transform targetToAim = UtilsClass.FindClosestTransform(this.transform, targetToShoot);
            UtilsClass.AimRotation(transform, targetToAim.position);

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
