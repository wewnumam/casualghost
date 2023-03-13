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
	public bool isDrawingLine = false;
	private int rifleEnemyCollideCounter;

	[Header("Hit Particle Properties")]
	public GameObject particleHitBlood;
	public GameObject particleHitNonBlood;

	[Header("Line Renderer Properties")]
	private LineRenderer lineRenderer;
	private Vector3 spawnPoint;

	private Rigidbody2D bulletPhysics;
	private float lifetimeElapsed = 0.0f;

	// Start is called before the first frame update
	private void Start() {
		bulletPhysics = GetComponent<Rigidbody2D>();

		if (isDrawingLine) {
			lineRenderer = GetComponentInChildren<LineRenderer>();

			// Save where was the bullet spawned for LineRenderer
			spawnPoint = transform.position;

			// LineRenderer setup: size of 2 for start and end position each
			lineRenderer.positionCount = 2;
		}
	}

	private void Update() {
		Life();
		DrawLineRenderer();
	}

	private void FixedUpdate() {
		// Fly with defined speed during its lifetime
		bulletPhysics.velocity = transform.right * flySpeed * 32.0f * Time.deltaTime;
	}

	public void SetBulletDamage(float bulletDamage) => _bulletDamage = bulletDamage;

	// Destroy projectiles when they collide with objects with collision
	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.CompareTag(Tags.ENEMY)) {
			GameObject ps = Instantiate(
				particleHitBlood,
				transform.position,
				transform.rotation
			);
			ps.GetComponentInChildren<ParticleSystem>().Play();
		} else {
			GameObject ps = Instantiate(
				particleHitNonBlood,
				transform.position,
				transform.rotation
			);
			ps.GetComponentInChildren<ParticleSystem>().Play();
		}

		Destroy(this.gameObject);
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (weaponType == WeaponType.RIFLE) {
			if (collider.gameObject.CompareTag(Tags.ENEMY)) {
				GameObject ps = Instantiate(
					particleHitBlood,
					transform.position,
					transform.rotation
				);
				ps.GetComponentInChildren<ParticleSystem>().Play();
				rifleEnemyCollideCounter++;
			} 
			
			if (collider.gameObject.CompareTag(Tags.TREE) ||
				collider.gameObject.CompareTag(Tags.BANYAN) ||
				collider.gameObject.CompareTag(Tags.ROOT) ||
				collider.gameObject.CompareTag(Tags.THORN_MINE) ||
				collider.gameObject.CompareTag(Tags.DECOY) ||
				collider.gameObject.CompareTag(Tags.CANNON)) {
				GameObject ps = Instantiate(
					particleHitNonBlood,
					transform.position,
					transform.rotation
				);
				ps.GetComponentInChildren<ParticleSystem>().Play();
				Destroy(this.gameObject);
			}
		}

		const int MAX_ENEMY_TO_COLLIDE = 2;
		if (rifleEnemyCollideCounter >= MAX_ENEMY_TO_COLLIDE) Destroy(gameObject); 
	}

	private void Life() {
		// Destroy projectile once it exceeds its lifetime
		lifetimeElapsed += Time.deltaTime;
		if (lifetimeElapsed >= lifetime) {
			Destroy(this.gameObject);
		}
	}

	// Draws LineRenderer when needed, set isDrawingLine after instantiating.
	private void DrawLineRenderer() {
		// If the spawned Projectile isn't set to draw, return
		if (!isDrawingLine) {
			return;
		}

		lineRenderer.SetPosition(0, spawnPoint);
		lineRenderer.SetPosition(1, transform.position);
	}
}
