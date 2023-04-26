using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	[Header("Movement Properties")]
    [SerializeField] private GameObject playerBody;
	[SerializeField] private float normalSpeed = 8f;  // The normal movement speed.
    [SerializeField] private float boostSpeed = 16f;  // The boosted movement speed.
    [SerializeField] private float boostDuration = 1f;  // The duration of the boost in seconds.
    [SerializeField] private float boostCooldown = 3f;  // The time it takes for the boost to recharge in seconds.

    private float currentSpeed;  // The current movement speed.
    private bool isBoosting = false;  // Whether the boost is currently active.

	private Rigidbody2D characterPhysics;
	[HideInInspector] public Vector2 movement;

	void Start() {
		characterPhysics = GetComponent<Rigidbody2D>();
		currentSpeed = normalSpeed;
        GetComponent<EchoEffect>().enabled = false;
	}

	void Update() {
        if (GetComponent<HealthSystem>().IsDie()) return;
        
		if (Input.GetKey(KeyCode.LeftShift) && !isBoosting) {
            StartCoroutine(Boost());
        }

		// Read input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Set movement vector
        if (Mathf.Approximately(horizontal, 0f) && Mathf.Approximately(vertical, 0f)) {
            // Player is not moving, set movement vector to zero
            movement = Vector2.zero;
            playerBody.GetComponent<Animator>().Play(AnimationTags.PLAYER_IDLE);
        }
        else {
            // Player is moving, set movement vector based on input
            movement = new Vector2(horizontal, vertical).normalized;
        }

        if (horizontal != 0 || vertical != 0) {
            if (GetComponent<Player>().IsPlayerTypeDefault()) {
                playerBody.GetComponent<Animator>().Play(AnimationTags.PLAYER_WALK);
            } else if (GetComponent<Player>().IsPlayerTypeTwo()) {
                playerBody.GetComponent<Animator>().Play(AnimationTags.PLAYER_WALK_TYPE_TWO);
            } else if (GetComponent<Player>().IsPlayerTypeThree()) {
                playerBody.GetComponent<Animator>().Play(AnimationTags.PLAYER_WALK_TYPE_THREE);
            }
        } else {
            playerBody.GetComponent<Animator>().Play(AnimationTags.PLAYER_IDLE);
        }
        
        // Player look at mouse
        float angle = UtilsClass.GetAngleFromVectorFloat((UtilsClass.GetMouseWorldPosition() - transform.position).normalized);
        Vector3 localScale = Vector3.one;
        if (angle > 90 && angle < 270) {
            localScale.x *= -1f;
        } else {
            localScale.x *= +1f;
        }
        playerBody.transform.localScale = localScale;
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
	}

	private IEnumerator Boost() {
        isBoosting = true;
        currentSpeed = boostSpeed;
        GetComponent<EchoEffect>().enabled = true;

        yield return new WaitForSeconds(boostDuration);

        currentSpeed = normalSpeed;
        GetComponent<EchoEffect>().enabled = false;

        yield return new WaitForSeconds(boostCooldown);

        isBoosting = false;
    }

    // Skill enhancer
    public void SpeedUp(float speedAddBy) {
        normalSpeed += speedAddBy;
        currentSpeed = normalSpeed;
        boostSpeed = normalSpeed * 2;
    }

    public void IncreaseSprintDuration(float durationAddBy) {
        boostDuration += durationAddBy;
    }
}
