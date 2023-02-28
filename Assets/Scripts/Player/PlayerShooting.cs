using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using EZCameraShake;
using static EnumsManager;

public class PlayerShooting : MonoBehaviour {
	[Header("Shooting Properties")]
	public GameObject[] bulletTypes;
	public Transform bulletSpawnPoint;
	public float bulletDamage = 1f;

	[Header("Weapon Properties")]
    [SerializeField] private WeaponType currentWeaponType = WeaponType.DEFAULT;
	[SerializeField] private int maxRound = 0;
	public int roundsLeft = 0;
	public float reloadTime = 1.5f;
	public float pullTriggerTime = 1f;
	private bool canShoot;

	void Start() {
        WeaponSwitch(0);
		canShoot = true;
	}

	void Update() {
		// Spawn bullet projectile
		if (Input.GetMouseButtonDown(0) && GameManager.Instance.IsGameStateGameplay() && Player.Instance.IsPlayerStateShoot()) {
			if (roundsLeft > 0 && canShoot) {
				 CameraShaker.Instance.ShakeOnce(10f, 10f, 0f, .25f);
				GetComponentInParent<Animator>().Play(AnimationTags.PLAYER_SHOOT);
				SoundManager.Instance.PlayShootSFX();

				GameObject b = Instantiate(
					bulletTypes[(int)currentWeaponType],
					bulletSpawnPoint.position,
					bulletSpawnPoint.rotation
				);

				b.GetComponent<Projectile>().bulletDamage = bulletDamage;

				roundsLeft--;
				canShoot = false;
				StartCoroutine(PullTrigger(pullTriggerTime));
			}
		}
		if (Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.Space)) {
			if (roundsLeft < maxRound) {
                StartCoroutine(ReloadSequence(reloadTime));
				GetComponentInParent<Animator>().Play(AnimationTags.PLAYER_RELOAD);
			}
		}
		
		UtilsClass.AimRotation(transform, UtilsClass.GetMouseWorldPosition());
        WeaponSelect();
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

	private IEnumerator PullTrigger(float time) {
        yield return new WaitForSeconds(time);
        canShoot = true;
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
