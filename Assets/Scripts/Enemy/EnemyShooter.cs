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
            AimRotation(targetToAim);

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
