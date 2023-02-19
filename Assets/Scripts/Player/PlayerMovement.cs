using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	[Header("Movement Properties")]
	public float normalSpeed = 8f;  // The normal movement speed.
    public float boostSpeed = 16f;  // The boosted movement speed.
    public float boostDuration = 1f;  // The duration of the boost in seconds.
    public float boostCooldown = 3f;  // The time it takes for the boost to recharge in seconds.

    private float currentSpeed;  // The current movement speed.
    private bool isBoosting = false;  // Whether the boost is currently active.

	private Rigidbody2D characterPhysics;
	private Vector2 velocity = Vector2.zero;

	void Start() {
		characterPhysics = GetComponent<Rigidbody2D>();
		currentSpeed = normalSpeed;
	}

	void Update() {
		if (Input.GetKey(KeyCode.LeftShift) && !isBoosting){
            StartCoroutine(Boost());
        }
	}

	// Update is called once per frame
	void FixedUpdate() {
		if (GameManager.Instance.IsGameStateGameplay()) {	
			// Movement from input axis
			velocity = new Vector3(
				Input.GetAxis("Horizontal") * currentSpeed,
				Input.GetAxis("Vertical") * currentSpeed,
				0
			);
		}
		
		// transform.Translate(velocity * Time.deltaTime, Space.World);
		characterPhysics.velocity = velocity * 32.0f * Time.deltaTime;
	}

	private IEnumerator Boost()
    {
        isBoosting = true;
        currentSpeed = boostSpeed;

        yield return new WaitForSeconds(boostDuration);

        currentSpeed = normalSpeed;

        yield return new WaitForSeconds(boostCooldown);

        isBoosting = false;
    }
}
