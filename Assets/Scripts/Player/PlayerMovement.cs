using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	[Header("Movement Properties")]
	public float moveSpeed = 8.0f;
	public Vector3 mouseWorldSpace = Vector3.zero;

	private Rigidbody2D characterPhysics;
	private Vector2 velocity = Vector2.zero;

	void Start() {
		characterPhysics = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void FixedUpdate() {
		if (GameManager.Instance.IsGameStateGameplay()) {	
			// Movement from input axis
			velocity = new Vector3(
				Input.GetAxis("Horizontal") * moveSpeed,
				Input.GetAxis("Vertical") * moveSpeed,
				0
			);
		}
		
		// transform.Translate(velocity * Time.deltaTime, Space.World);
		characterPhysics.velocity = velocity * 32.0f * Time.deltaTime;
	}
}
