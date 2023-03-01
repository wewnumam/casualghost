using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using EZCameraShake;
using static EnumsManager;

public class PlayerShooting : MonoBehaviour {
	[Header("Shooting Properties")]
	[SerializeField] private GameObject[] bulletTypes;
	[SerializeField] private Transform bulletSpawnPoint;
	[SerializeField] private float _bulletDamage = 1f;
	public float bulletDamage { get => _bulletDamage; }

	[Header("Weapon Properties")]
    [SerializeField] private WeaponType currentWeaponType = WeaponType.DEFAULT;
	[SerializeField] private int maxRound = 0;

	private int _roundsLeft = 0;
	public int roundsLeft { get => _roundsLeft; }

	[SerializeField] private float _reloadTime = 1.5f;
	public float reloadTime { get => _reloadTime; }

	[SerializeField] private float _pullTriggerTime = 1f;
	public float pullTriggerTime { get => _pullTriggerTime; }

	private bool canShoot;

	void Start() {
        WeaponSwitch(0);
		canShoot = true;
	}

	void Update() {
		// Spawn bullet projectile
		if (Input.GetMouseButtonDown(0) && GameManager.Instance.IsGameStateGameplay() && Player.Instance.IsPlayerStateShoot()) {
			if (_roundsLeft > 0 && canShoot) {
				Shoot();
			}
		}
		if (Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.Space)) {
			if (_roundsLeft < maxRound) {
                StartCoroutine(ReloadSequence(_reloadTime));
				GetComponentInParent<Animator>().Play(AnimationTags.PLAYER_RELOAD);
			}
		}
		
		UtilsClass.AimRotation(transform, UtilsClass.GetMouseWorldPosition());
        WeaponSelect();
	}

	void Shoot() {
		CameraShaker.Instance.ShakeOnce(10f, 10f, 0f, .25f);
		GetComponentInParent<Animator>().Play(AnimationTags.PLAYER_SHOOT);
		SoundManager.Instance.PlayShootSFX();

		GameObject b = Instantiate(
			bulletTypes[(int)currentWeaponType],
			bulletSpawnPoint.position,
			bulletSpawnPoint.rotation
		);

		b.GetComponent<Projectile>().SetBulletDamage(_bulletDamage);

		_roundsLeft--;
		canShoot = false;
		StartCoroutine(PullTrigger(pullTriggerTime));
	}

	public void SetBulletDamage(float bulletDamage) => _bulletDamage = bulletDamage; 
	public void SetReloadTime(float reloadTime) => _reloadTime = reloadTime; 
	public void SetPullTriggerTime(float pullTriggerTime) => _pullTriggerTime = pullTriggerTime; 

	private void WeaponSwitch(int idx) {
        currentWeaponType = bulletTypes[idx].GetComponent<Projectile>().weaponType;
		switch (
			bulletTypes[idx]
				.GetComponent<Projectile>()
				.weaponType) {
			case WeaponType.DEFAULT:
				maxRound = 5;
				_reloadTime = 1.5f;
				break;
			case WeaponType.RIFLE:
				maxRound = 31;
				_reloadTime = 1.3f;
				break;
			default:
				break;
		}
        _roundsLeft = maxRound;
	}

    private IEnumerator ReloadSequence(float time) {
        yield return new WaitForSeconds(time);
        _roundsLeft = maxRound;
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
