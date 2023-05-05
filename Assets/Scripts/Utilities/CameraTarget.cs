using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour {
    [SerializeField] private Transform player;
    [SerializeField] private float threshold;
    private Vector3 targetPosition;

    void Update() {
        if (player == null) {
            if (Player.Instance == null) return;
            player = Player.Instance.transform;
        } else {
            targetPosition = (player.position + UtilsClass.GetMouseWorldPosition(GameManager.Instance.mainCamera)) / 2f;
            targetPosition.x = Mathf.Clamp(targetPosition.x, -threshold + player.position.x, threshold + player.position.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, -threshold + player.position.y, threshold + player.position.y);

            transform.position = targetPosition;
        }
    }
}
