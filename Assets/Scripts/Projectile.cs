using UnityEngine;
using static EnumsManager;

public class Projectile : MonoBehaviour {
	[Header("Bullet Type")]
	public WeaponType weaponType = WeaponType.DEFAULT;
	[SerializeField] private float _bulletDamage = 1f;
	public float bulletDamage { get => _bulletDamage; }

	[Header("Behaviour")]
	[SerializeField] private float flySpeed = 12.0f;
	[SerializeField] private float lifetime = 4.0f;

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

	public void SetBulletDamage(float bulletDamage) => _bulletDamage = bulletDamage;

	// Destroy projectiles when they collide with objects with collision
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
