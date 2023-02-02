using System.Collections;
using UnityEngine;

public class CharacterShooting : MonoBehaviour {
	[Header("Shooting Properties")]
	public GameObject[] bulletTypes;
	public Transform bulletSpawnPoint;

	[Header("Weapon Properties")]
    [SerializeField] private WeaponType currentWeaponType = WeaponType.DEFAULT;
	[SerializeField] private int maxRound = 0;
	[SerializeField] private int roundsLeft = 0;
	[SerializeField] private float reloadTime = 3.0f;

	void Start() {
        WeaponSwitch(0);
	}

	void Update() {
		// Spawn bullet projectile
		if (Input.GetMouseButtonDown(0)) {
			if (roundsLeft > 0) {
				GameObject b = Instantiate(
					bulletTypes[(int)currentWeaponType],
					bulletSpawnPoint.position,
					bulletSpawnPoint.rotation
				);

				roundsLeft--;
			}
		}
		if (Input.GetKeyDown(KeyCode.R)) {
			if (roundsLeft < maxRound) {
                StartCoroutine(ReloadSequence(reloadTime));
			}
		}

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
				reloadTime = 3.0f;
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
