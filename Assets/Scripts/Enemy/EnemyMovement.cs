using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    [Header("Follow Target Properties")]
    private Transform[] targetToFollow;

    [Header("Movement Settings")]
    [SerializeField] private float _maxSpeed;
    public float maxSpeed { get => _maxSpeed; }
    public void SetMaxSpeed(float maxSpeed) => _maxSpeed = maxSpeed;

    void Update() {
        GameObject[] decoys = GameObject.FindGameObjectsWithTag(Tags.DECOY);

        // Prioritize to follow decoy
        if (decoys.Length > 0) {
            Transform[] decoysToFollow = UtilsClass.GetGameObjectsTransform(decoys);
            FollowTarget(UtilsClass.FindClosestTransform(this.transform, decoysToFollow));
        } else {
            GameObject[] canons = GameObject.FindGameObjectsWithTag(Tags.CANNON);
            
            // Check if any canons were found 
            if (canons.Length > 0) {
                targetToFollow = UtilsClass.GetGameObjectsTransform(canons, 2);
                targetToFollow[canons.Length] = GameObject.FindWithTag(Tags.PLAYER).transform;
                targetToFollow[canons.Length + 1] = GameObject.FindWithTag(Tags.BANYAN).transform;
            } else {
                targetToFollow = new Transform[] {
                    GameObject.FindWithTag(Tags.PLAYER).transform,
                    GameObject.FindWithTag(Tags.BANYAN).transform
                };
            }

            FollowTarget(UtilsClass.FindClosestTransform(this.transform, targetToFollow));
        }
    }

    void FollowTarget(Transform target) {
        // Enemy follow target
        Vector3 moveDir = (target.position - transform.position).normalized;
        GetComponent<Rigidbody2D>().AddForce(moveDir * _maxSpeed);

        // Limit the velocity to a maximum value
        float currentSpeed = GetComponent<Rigidbody2D>().velocity.magnitude;
        if (currentSpeed > _maxSpeed) {
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity.normalized * _maxSpeed;
        }

        if (currentSpeed == 0) {
            GetComponent<Enemy>().PlayEnemyIdleAnimation();
        } else {
            GetComponent<Enemy>().PlayEnemyWalkAnimation();
        }

        // Enemy look at target
        float angle = UtilsClass.GetAngleFromVectorFloat((target.position - transform.position).normalized);
        Vector3 localScale = Vector3.one;
		if (angle > 90 && angle < 270) {
			localScale.x *= -1f;
		} else {
			localScale.x *= +1f;
		}
		transform.localScale = localScale;
    }
}
