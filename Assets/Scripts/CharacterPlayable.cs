using UnityEngine;

public class CharacterPlayable : MonoBehaviour {
	[Header("Movement Properties")]
	public float moveSpeed = 8.0f;
	public Vector3 mouseWorldSpace = Vector3.zero;

	private Rigidbody2D characterPhysics;
	private Vector2 velocity = Vector2.zero;

	void Start() {
		characterPhysics = GetComponent<Rigidbody2D>();
	}

	void Update() {
		// Rotation facing towards mouse cursor
		Vector3 aimDirection = (UtilsClass.GetMouseWorldPosition() - transform.position).normalized;
		float angle = UtilsClass.GetAngleFromVectorFloat(aimDirection);
		transform.eulerAngles = new Vector3(0, 0, angle);
	}

	// Update is called once per frame
	void FixedUpdate() {
		// Movement from input axis
		velocity = new Vector3(
			Input.GetAxis("Horizontal") * moveSpeed,
			Input.GetAxis("Vertical") * moveSpeed,
			0
		);
		// transform.Translate(velocity * Time.deltaTime, Space.World);
		characterPhysics.velocity = velocity * 32.0f * Time.deltaTime;
	}
}
