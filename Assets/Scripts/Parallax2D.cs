using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax2D : MonoBehaviour {
    [SerializeField] private float parallaxSpeed = 0.5f; // The speed at which the object moves relative to the camera
    [SerializeField] private float zPosition = 0.0f; // The depth at which the object appears

    private Vector3 lastCameraPosition;

    void Start() {
        lastCameraPosition = Camera.main.transform.position;
    }

    void LateUpdate() {
        Vector3 deltaMovement = Camera.main.transform.position - lastCameraPosition;
        Vector3 parallaxMovement = new Vector3(deltaMovement.x * parallaxSpeed, deltaMovement.y * parallaxSpeed, zPosition);
        gameObject.transform.position += parallaxMovement;
        lastCameraPosition = Camera.main.transform.position;
    }
}
