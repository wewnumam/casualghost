using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    [Header("Caching Components")]
    [SerializeField] private Enemy enemy;
    [SerializeField] private new Rigidbody2D rigidbody2D;

    [Header("Follow Target Properties")]
    private Transform[] targetToFollow;
    private bool isCollideWithTarget;

    [Header("Movement Settings")]
    [SerializeField] private float _maxSpeed;
    public float maxSpeed { get => _maxSpeed; }
    public void SetMaxSpeed(float maxSpeed) => _maxSpeed = maxSpeed;

    void Update() {
        if (Player.Instance == null) return;
        
        if (enemy.IsEnemyTypeDefault()) {
            EnemyDefaultTarget();
        } else if (enemy.IsEnemyTypeRunner()) {
            EnemyRunnerTarget();
        } else if (enemy.IsEnemyTypeGiant()) {
            EnemyGiantTarget();
        } else if (enemy.IsEnemyTypeShooter()) {
            EnemyDefaultTarget();
        } else if (enemy.IsEnemyTypeBoss()) {
            EnemyBossTarget();
        }

        if (isCollideWithTarget) {
            enemy.PlayEnemyAttackAnimation();
        } else {
            if (!enemy.IsEnemyTypeShooter()) {
                enemy.PlayEnemyWalkAnimation();
            }
        }
    }

    void EnemyDefaultTarget() {
        // Prioritize to follow decoy
        if (GameManager.Instance.currentDecoys.Count > 0) {
            Transform[] decoysToFollow = UtilsClass.GetGameObjectsTransform(GameManager.Instance.currentDecoys.ToArray());
            FollowTarget(UtilsClass.FindClosestTransform(this.transform, decoysToFollow));
        } else {
            // Check if any canons were found 
            if (GameManager.Instance.currentCannons.Count > 0) {
                targetToFollow = UtilsClass.GetGameObjectsTransform(GameManager.Instance.currentCannons.ToArray(), 2);
                targetToFollow[GameManager.Instance.currentCannons.Count] = Player.Instance.transform;
                targetToFollow[GameManager.Instance.currentCannons.Count + 1] = BanyanDefenseManager.Instance.transform;
            } else {
                targetToFollow = new Transform[] {
                    Player.Instance.transform,
                    BanyanDefenseManager.Instance.transform
                };
            }

            FollowTarget(UtilsClass.FindClosestTransform(this.transform, targetToFollow));
        }
    }

    void EnemyRunnerTarget() {
        // Prioritize to follow decoy
        if (GameManager.Instance.currentDecoys.Count > 0) {
            Transform[] decoysToFollow = UtilsClass.GetGameObjectsTransform(GameManager.Instance.currentDecoys.ToArray());
            FollowTarget(UtilsClass.FindClosestTransform(this.transform, decoysToFollow));
        } else {
            if (GameManager.Instance.currentCannons.Count > 0) {
                targetToFollow = UtilsClass.GetGameObjectsTransform(GameManager.Instance.currentCannons.ToArray());
            } else if (GameManager.Instance.currentRoots.Count > 0) {
                targetToFollow = UtilsClass.GetGameObjectsTransform(GameManager.Instance.currentRoots.ToArray());
            } else if (GameManager.Instance.currentThornMine.Count > 0) {
                targetToFollow = UtilsClass.GetGameObjectsTransform(GameManager.Instance.currentThornMine.ToArray());
            } else {
                targetToFollow = new Transform[] {
                    Player.Instance.transform,
                };
            }

            FollowTarget(UtilsClass.FindClosestTransform(this.transform, targetToFollow));
        }
    }

    void EnemyGiantTarget() {
        // Prioritize to follow decoy
        if (GameManager.Instance.currentDecoys.Count > 0) {
            Transform[] decoysToFollow = UtilsClass.GetGameObjectsTransform(GameManager.Instance.currentDecoys.ToArray());
            FollowTarget(UtilsClass.FindClosestTransform(this.transform, decoysToFollow));
        } else {
            targetToFollow = new Transform[] {
                BanyanDefenseManager.Instance.transform,
            };

            FollowTarget(UtilsClass.FindClosestTransform(this.transform, targetToFollow));
        }
    }

    void EnemyBossTarget() {
        targetToFollow = new Transform[] {
            BanyanDefenseManager.Instance.transform,
        };

        FollowTarget(UtilsClass.FindClosestTransform(this.transform, targetToFollow));
    }

    void FollowTarget(Transform target) {
        if (enemy.healthSystem.IsDie()) {
            rigidbody2D.velocity = Vector2.zero;
            return;
        }

        // Enemy follow target
        Vector3 moveDir = (target.position - transform.position).normalized;
        rigidbody2D.AddForce(moveDir * _maxSpeed);

        // Limit the velocity to a maximum value
        float currentSpeed = rigidbody2D.velocity.magnitude;
        if (currentSpeed > _maxSpeed) {
            rigidbody2D.velocity = rigidbody2D.velocity.normalized * _maxSpeed;
        }

        // Enemy look at target
        float angle = UtilsClass.GetAngleFromVectorFloat((target.position - transform.position).normalized);
        // Vector3 localScale = Vector3.one;
		if (angle > 90 && angle < 270) {
			transform.localRotation = Quaternion.Euler(0, 180, 0);
		} else {
			transform.localRotation = Quaternion.Euler(0, 0, 0);
		}
		// transform.localScale = localScale;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag(Tags.TREE)) {
            if (!enemy.IsEnemyTypeBoss()) {
                StartCoroutine(collision.gameObject.GetComponent<Tree>().HideTreeTemporary(2f));
                collision.gameObject.GetComponent<Tree>().SwitchToDarkTree();
            }
        }

        if (targetToFollow == null) return;

        foreach (var t in targetToFollow) {
            if (t == null) {
                isCollideWithTarget = false;
                break;
            }

            if ((collision.gameObject.CompareTag(t.gameObject.tag) || 
                collision.gameObject.CompareTag(Tags.ROOT) ||
                collision.gameObject.CompareTag(Tags.DECOY) ||
                collision.gameObject.CompareTag(Tags.CANNON))) {
                isCollideWithTarget = true;
                break;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        if (targetToFollow == null) return;
        
        foreach (var t in targetToFollow) {
            if (t == null) {
                isCollideWithTarget = false;
                break;
            }

            if ((collision.gameObject.CompareTag(t.gameObject.tag) ||
                collision.gameObject.CompareTag(Tags.ROOT) ||
                collision.gameObject.CompareTag(Tags.DECOY) ||
                collision.gameObject.CompareTag(Tags.CANNON))) {
                isCollideWithTarget = false;
                break;
            }
        }
    }
}
