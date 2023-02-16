using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour {
	[Header("Shooting Properties")]
	public GameObject[] bulletTypes;
	public Transform bulletSpawnPoint;

	[Header("Weapon Properties")]
    [SerializeField] private WeaponType currentWeaponType = WeaponType.DEFAULT;
	[SerializeField] private int maxRound = 0;
	public int roundsLeft = 0;
	[SerializeField] private float reloadTime = 1.5f;

	void Start() {
        WeaponSwitch(0);
	}

	void Update() {
		// Spawn bullet projectile
		if (Input.GetMouseButtonDown(0)) {
			if (roundsLeft > 0) {
				GetComponentInParent<Animator>().Play(AnimationTags.PLAYER_SHOOT);
				SoundManager.Instance.PlayShootSFX();

				GameObject b = Instantiate(
					bulletTypes[(int)currentWeaponType],
					bulletSpawnPoint.position,
					bulletSpawnPoint.rotation
				);

				roundsLeft--;
			}
		}
		if (Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.Space)) {
			if (roundsLeft < maxRound) {
                StartCoroutine(ReloadSequence(reloadTime));
				GetComponentInParent<Animator>().Play(AnimationTags.PLAYER_RELOAD);
			}
		}
		
		AimRotation();
        WeaponSelect();
	}

	void AimRotation() {
		// Rotation facing towards mouse cursor
		Vector3 aimDirection = (UtilsClass.GetMouseWorldPosition() - transform.position).normalized;
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

	private void WeaponSwitch(int idx) {
        currentWeaponType = bulletTypes[idx].GetComponent<Projectile>().weaponType;
		switch (
			bulletTypes[idx]
				.GetComponent<Projectile>()
				.weaponType) {
			case WeaponType.DEFAULT:
				maxRound = 5;
				reloadTime = 1.5f;
				break;
			case WeaponType.RIFLE:
				maxRound = 31;
				reloadTime = 1.3f;
				break;
			default:
				break;
		}
        roundsLeft = maxRound;
	}

    private IEnumerator ReloadSequence(float time) {
        yield return new WaitForSeconds(time);
        roundsLeft = maxRound;
    }

    private void WeaponSelect() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            WeaponSwitch(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            WeaponSwitch(1);
        }
    }

}

public enum WeaponType {
	DEFAULT,
	RIFLE
}
