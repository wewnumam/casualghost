using UnityEngine;

public class CharacterShooting : MonoBehaviour {
    [Header("Shooting Properties")]
    public GameObject[] bulletTypes;
    public Transform bulletSpawnPoint;

    private int roundsLeft = 0;

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {
        // Spawn bullet projectile
        if (Input.GetMouseButtonDown(0)) {
            GameObject b = Instantiate(
                bulletTypes[0],
                bulletSpawnPoint.position,
                bulletSpawnPoint.rotation
            );
        }
	}
}
