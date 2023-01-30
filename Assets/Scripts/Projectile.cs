using UnityEngine;

public class Projectile : MonoBehaviour {
    public float flySpeed = 12.0f;
    public float lifetime = 10.0f;

    private Rigidbody2D bulletPhysics;

	// Start is called before the first frame update
	void Start() {
        bulletPhysics = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void FixedUpdate() {
        bulletPhysics.velocity = transform.up * flySpeed * 32.0f * Time.deltaTime;
	}

    void OnCollisionEnter(Collision collision) {
        Debug.Log("bullet murdered");
        Destroy(this.gameObject);
    }

    public void SetAngle(Vector3 direction) {
        transform.eulerAngles = direction;
    }
}
