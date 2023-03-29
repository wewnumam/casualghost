using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingSpear : MonoBehaviour {
    [SerializeField] private float rotateSpeed;
    private Transform playerTransform;

    void Awake() {
        playerTransform = GameObject.FindGameObjectWithTag(Tags.PLAYER).transform;
    }

    void Update() {
        transform.position = playerTransform.position;
        transform.RotateAround(playerTransform.position, new Vector3(0, 0, -1), rotateSpeed * Time.deltaTime);
    }
}
