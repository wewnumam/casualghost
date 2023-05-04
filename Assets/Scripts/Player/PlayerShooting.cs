using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EZCameraShake;

public class PlayerShooting : MonoBehaviour {
	[Header("Caching Components")]
    [SerializeField] private Animator handAnimator;
	
	[Header("Weapon Properties")]
    [SerializeField] private EnumsManager.WeaponType _currentWeaponType = EnumsManager.WeaponType.DEFAULT;
    public EnumsManager.WeaponType currentWeaponType { get => _currentWeaponType; }
	[SerializeField] private List<PlayerWeapon> playerWeapon;
	[SerializeField] private PlayerWeapon currentPlayerWeapon;

	[Header("Shooting Properties")]
	private int _roundsLeft = 0;
	public int roundsLeft { get => _roundsLeft; }
	public int maxRound;
	private bool canShoot;
	private bool isReload;

	[Header("Bullet Instantiate Properties")]
	[SerializeField] private GameObject[] bulletTypes;
	[SerializeField] private Transform bulletSpawnPoint;
	private GameObject currentProjectile;

	void Start() {
		if (PlayerPrefs.GetInt(PlayerPrefsKeys.WEAPON) == PlayerPrefsValues.WEAPON_DEFAULT) {
        	_currentWeaponType = EnumsManager.WeaponType.DEFAULT;
		} else if (PlayerPrefs.GetInt(PlayerPrefsKeys.WEAPON) == PlayerPrefsValues.WEAPON_SHOTGUN) {
        	_currentWeaponType = EnumsManager.WeaponType.SHOTGUN;
		} else if (PlayerPrefs.GetInt(PlayerPrefsKeys.WEAPON) == PlayerPrefsValues.WEAPON_RIFLE) {
        	_currentWeaponType = EnumsManager.WeaponType.RIFLE;
		}

		ResetPlayerWeapon();

		canShoot = true;
	}

	void Update() {
		maxRound = currentPlayerWeapon.maxRound;
		
		if (_roundsLeft == 0 && !isReload && canShoot) {
			handAnimator.Play(AnimationTags.PLAYER_AMMO_EMPTY);
		}

		// Spawn bullet projectile when left mouse button is pressed
		if (Input.GetMouseButtonDown(0) && GameManager.Instance.IsGameStateGameplay() && Player.Instance.IsPlayerStateShoot()) {
			if (_roundsLeft > 0 && canShoot && !isReload) {
				Shoot();
			}
		}
		// Reload when R or Space key is pressed and there are rounds left
		if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Space)) {
			if (_roundsLeft < currentPlayerWeapon.maxRound && !isReload) {
                Reload();
			}
		}

		// Auto reload
		if (_roundsLeft == 0 && !isReload) {
            Reload();
		}
		
		UtilsClass.AimRotation(transform, UtilsClass.GetMouseWorldPosition(GameManager.Instance.mainCamera)); // Rotate player towards the mouse cursor
	}

	void Reload() {
		StartCoroutine(ReloadSequence(currentPlayerWeapon.reloadTime));
		handAnimator.SetFloat(AnimationTags.PLAYER_RELOAD_TIME, 1 / currentPlayerWeapon.reloadTime); 
		handAnimator.Play(AnimationTags.PLAYER_RELOAD);
	}

	void ResetPlayerWeapon() {
        for (int i = 0; i < playerWeapon.Count; i++) {
            if (playerWeapon[i].weaponType == _currentWeaponType) {
				currentPlayerWeapon = playerWeapon[i];
				_roundsLeft = currentPlayerWeapon.maxRound;
				break;
			}
		}
	}

	void Shoot() {
		StartCoroutine(PullTrigger(currentPlayerWeapon.pullTriggerTime)); // Delay before player can shoot again
		CameraShaker.Instance.ShakeOnce(10f, 10f, 0f, .25f); // Shake camera when shooting
		handAnimator.Play(AnimationTags.PLAYER_SHOOT); // Play shooting animation
		
		GameObject b = Instantiate(
			currentPlayerWeapon.bulletPrefab, // Instantiate bullet of current weapon type
			bulletSpawnPoint.position,
			bulletSpawnPoint.rotation
		);

		for (int i = 0; i < b.transform.childCount; i++) {
			b.GetComponentsInChildren<Projectile>()[i].SetBulletDamage(currentPlayerWeapon.bulletDamage); // Set bullet damage based on bullet damage property
		}
	}

	// Setters for properties that can be modified at runtime
	public void SetReloadTime(float reloadTime) => currentPlayerWeapon.reloadTime = reloadTime; 
	public void SetPullTriggerTime(float pullTriggerTime) => currentPlayerWeapon.pullTriggerTime = pullTriggerTime; 

	// Switch between different weapon types
	public void WeaponSwitch(EnumsManager.WeaponType weaponType) {
		_currentWeaponType = weaponType;
		ResetPlayerWeapon();
        _roundsLeft = currentPlayerWeapon.maxRound;
	}

    private IEnumerator ReloadSequence(float time) {
		isReload = true;
        yield return new WaitForSeconds(time);
		isReload = false;
        _roundsLeft = currentPlayerWeapon.maxRound;
    }

	private IEnumerator PullTrigger(float time) {
		_roundsLeft--;
		canShoot = false;
        yield return new WaitForSeconds(time);
        canShoot = true;
    }

	// Skill Enhancer
	public void FastReload(float reloadTimeDivideBy) {
		currentPlayerWeapon.reloadTime /= reloadTimeDivideBy;
	} 

	public void FastTrigger(float pullTriggerTimeDivideBy) {
		currentPlayerWeapon.pullTriggerTime /= pullTriggerTimeDivideBy;
	} 

	public void IncreaseBulletDamage(float bulletDamageAddBy) {
		currentPlayerWeapon.bulletDamage += bulletDamageAddBy;
	} 
}

[System.Serializable]
public class PlayerWeapon {
    public EnumsManager.WeaponType weaponType;
	public int maxRound;
	public float reloadTime;
	public float pullTriggerTime;
	public GameObject bulletPrefab;
	public float bulletDamage;
}
