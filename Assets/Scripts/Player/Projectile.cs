using UnityEngine;

public class Projectile : MonoBehaviour {
	[Header("Bullet Type")]
	public WeaponType weaponType = WeaponType.DEFAULT;
	public float bulletDamage = 1f;

	[Header("Behaviour")]
	public float flySpeed = 12.0f;
	public float lifetime = 4.0f;

	private Rigidbody2D bulletPhysics;
	private float lifetimeElapsed = 0.0f;

	// Start is called before the first frame update
	void Start() {
		bulletPhysics = GetComponent<Rigidbody2D>();
	}

	void Update() {
		Life();
	}

	void FixedUpdate() {
		// Fly with defined speed during its lifetime
		bulletPhysics.velocity = transform.right * flySpeed * 32.0f * Time.deltaTime;
	}

	void OnCollisionEnter2D(Collision2D collision) {
		Destroy(this.gameObject);
	}

	void Life() {
		// Destroy projectile once it exceeds its lifetime
		lifetimeElapsed += Time.deltaTime;
		if (lifetimeElapsed >= lifetime) {
			Destroy(this.gameObject);
		}
	}
}
