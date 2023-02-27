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
	private Vector2 movement;

	void Start() {
		characterPhysics = GetComponent<Rigidbody2D>();
		currentSpeed = normalSpeed;
	}

	void Update() {
		if (Input.GetKey(KeyCode.LeftShift) && !isBoosting){
            StartCoroutine(Boost());
        }

		// Read input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Set movement vector
        if (Mathf.Approximately(horizontal, 0f) && Mathf.Approximately(vertical, 0f)) {
            // Player is not moving, set movement vector to zero
            movement = Vector2.zero;
        }
        else {
            // Player is moving, set movement vector based on input
            movement = new Vector2(horizontal, vertical).normalized;
        }
	}

	// Update is called once per frame
	void FixedUpdate() {
		if (GameManager.Instance.IsGameStateGameplay()) {	
			// Move player
			characterPhysics.MovePosition(characterPhysics.position + movement * currentSpeed * Time.fixedDeltaTime);

			 // Reset velocity if player is not moving
			if (movement == Vector2.zero) {
				characterPhysics.velocity = Vector2.zero;
			}
		}
		
		// transform.Translate(velocity * Time.deltaTime, Space.World);
		// characterPhysics.velocity = velocity * 32.0f * Time.deltaTime;
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

    public void SetCurrentSpeed(float currentSpeed) {
        this.currentSpeed = currentSpeed;
    }
}
