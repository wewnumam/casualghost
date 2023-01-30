using UnityEngine;

public class CharacterController : MonoBehaviour {
	[Header("Movement Properties")]
	public float moveSpeed = 8.0f;
    public Vector3 mouseWorldSpace = Vector3.zero;

	private Vector2 velocity = Vector2.zero;
    [SerializeField] private Vector3 screenDirection = Vector3.forward;

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {
        // Movement from input axis
        velocity = new Vector3(
            Input.GetAxis("Horizontal") * moveSpeed,
            Input.GetAxis("Vertical") * moveSpeed,
            0
        );
        transform.Translate(velocity * Time.deltaTime, Space.World);
        
        // Rotation facing towards mouse cursor
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = transform.position.z;
        mouseWorldSpace = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        transform.LookAt(mouseWorldSpace, screenDirection);
        transform.eulerAngles = new Vector3(0, 0, -transform.eulerAngles.z);
	}
}
