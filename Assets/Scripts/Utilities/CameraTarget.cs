using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour {
    [SerializeField] private Transform player;
    [SerializeField] private float threshold;

    void Update() {
        if (player == null) {
            if (GameObject.FindGameObjectWithTag(Tags.PLAYER) == null) return;
            player = GameObject.FindGameObjectWithTag(Tags.PLAYER).transform;
        } else {
            Vector3 targetPosition = (player.position + UtilsClass.GetMouseWorldPosition()) / 2f;
            targetPosition.x = Mathf.Clamp(targetPosition.x, -threshold + player.position.x, threshold + player.position.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, -threshold + player.position.y, threshold + player.position.y);

            transform.position = targetPosition;
        }
    }
}
