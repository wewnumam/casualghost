using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using EZCameraShake;
using static EnumsManager;

public class PlayerShooting : MonoBehaviour {
	

	[Header("Weapon Properties")]
    [SerializeField] private WeaponType currentWeaponType = WeaponType.DEFAULT;
	[SerializeField] private int maxRound = 0;

	[Header("Shooting Properties")]
	[SerializeField] private float _bulletDamage = 1f;
	public float bulletDamage { get => _bulletDamage; }
	private int _roundsLeft = 0;
	public int roundsLeft { get => _roundsLeft; }
	[SerializeField] private float _reloadTime = 1.5f;
	public float reloadTime { get => _reloadTime; }
	[SerializeField] private float _pullTriggerTime = 1f;
	public float pullTriggerTime { get => _pullTriggerTime; }
	private bool canShoot;

	[Header("Bullet Instantiate Properties")]
	[SerializeField] private GameObject[] bulletTypes;
	[SerializeField] private Transform bulletSpawnPoint;

	void Start() {
        WeaponSwitch(0); // Set default weapon type
		canShoot = true;
	}

	void Update() {
		// Spawn bullet projectile when left mouse button is pressed
		if (Input.GetMouseButtonDown(0) && GameManager.Instance.IsGameStateGameplay() && Player.Instance.IsPlayerStateShoot()) {
			if (_roundsLeft > 0 && canShoot) {
				Shoot();
			}
		}
		// Reload when R or Space key is pressed and there are rounds left
		if (Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.Space)) {
			if (_roundsLeft < maxRound) {
                StartCoroutine(ReloadSequence(_reloadTime));
				GetComponentInParent<Animator>().Play(AnimationTags.PLAYER_RELOAD);
			}
		}
		
		UtilsClass.AimRotation(transform, UtilsClass.GetMouseWorldPosition()); // Rotate player towards the mouse cursor
        WeaponSelect(); // Switch between different weapons
	}

	void Shoot() {
		CameraShaker.Instance.ShakeOnce(10f, 10f, 0f, .25f); // Shake camera when shooting
		GetComponentInParent<Animator>().Play(AnimationTags.PLAYER_SHOOT); // Play shooting animation
		SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.PLAYER_SHOOT_WEAPON_DEFAULT); // Play shooting sound effect

		GameObject b = Instantiate(
			bulletTypes[(int)currentWeaponType], // Instantiate bullet of current weapon type
			bulletSpawnPoint.position,
			bulletSpawnPoint.rotation
		);

		b.GetComponent<Projectile>().SetBulletDamage(_bulletDamage); // Set bullet damage based on bullet damage property

		_roundsLeft--;
		canShoot = false;
		StartCoroutine(PullTrigger(pullTriggerTime)); // Delay before player can shoot again
	}

	// Setters for properties that can be modified at runtime
	public void SetBulletDamage(float bulletDamage) => _bulletDamage = bulletDamage; 
	public void SetReloadTime(float reloadTime) => _reloadTime = reloadTime; 
	public void SetPullTriggerTime(float pullTriggerTime) => _pullTriggerTime = pullTriggerTime; 

	// Switch between different weapon types
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
