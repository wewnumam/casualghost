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
                targetToFollow[canons.Length] = Player.Instance.transform;
                targetToFollow[canons.Length + 1] = BanyanDefenseManager.Instance.transform;
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
        GameObject[] decoys = GameObject.FindGameObjectsWithTag(Tags.DECOY);

        // Prioritize to follow decoy
        if (decoys.Length > 0) {
            Transform[] decoysToFollow = UtilsClass.GetGameObjectsTransform(decoys);
            FollowTarget(UtilsClass.FindClosestTransform(this.transform, decoysToFollow));
        } else {
            GameObject[] canons = GameObject.FindGameObjectsWithTag(Tags.CANNON);
            GameObject[] roots = GameObject.FindGameObjectsWithTag(Tags.ROOT);
            GameObject[] thornmine = GameObject.FindGameObjectsWithTag(Tags.THORN_MINE);
            
            if (canons.Length > 0) {
                targetToFollow = UtilsClass.GetGameObjectsTransform(canons);
            } else if (roots.Length > 0) {
                targetToFollow = UtilsClass.GetGameObjectsTransform(roots);
            } else if (thornmine.Length > 0) {
                targetToFollow = UtilsClass.GetGameObjectsTransform(thornmine);
            } else {
                targetToFollow = new Transform[] {
                    Player.Instance.transform,
                };
            }

            FollowTarget(UtilsClass.FindClosestTransform(this.transform, targetToFollow));
        }
    }

    void EnemyGiantTarget() {
        GameObject[] decoys = GameObject.FindGameObjectsWithTag(Tags.DECOY);

        // Prioritize to follow decoy
        if (decoys.Length > 0) {
            Transform[] decoysToFollow = UtilsClass.GetGameObjectsTransform(decoys);
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
            if ((collision.gameObject.CompareTag(t.gameObject.tag) || 
                collision.gameObject.CompareTag(Tags.ROOT) ||
                collision.gameObject.CompareTag(Tags.DECOY) ||
                collision.gameObject.CompareTag(Tags.CANNON)) && t != null) {
                isCollideWithTarget = true;
                break;
            }
        }
    }

    

    void OnCollisionExit2D(Collision2D collision) {
        if (targetToFollow == null) return;
        
        foreach (var t in targetToFollow) {
            if ((collision.gameObject.CompareTag(t.gameObject.tag) ||
                collision.gameObject.CompareTag(Tags.ROOT) ||
                collision.gameObject.CompareTag(Tags.DECOY) ||
                collision.gameObject.CompareTag(Tags.CANNON)) && t != null) {
                isCollideWithTarget = false;
                break;
            }
        }
    }
}
