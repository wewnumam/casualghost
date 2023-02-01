using UnityEngine;

public class CameraRig : MonoBehaviour {
	[Header("Follow Target")]
	public Transform followTarget;
	public float followSpeed = 4.0f;

	void FixedUpdate() {
		FollowTarget();
	}

	public void FollowTarget() {
		if (followTarget) {
			transform.position = Vector3.Lerp(
				transform.position,
				followTarget.position,
				followSpeed * Time.deltaTime
			);
		}
	}
}
