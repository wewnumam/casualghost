using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour {
    [SerializeField] private float moveSpeed;

    void Update() {
        Vector3 currentPosition = transform.position;
        if (GetComponent<AreaEffector2D>().forceMagnitude > 0) {
            currentPosition.x += moveSpeed * Time.deltaTime;
        } else {
            currentPosition.x -= moveSpeed * Time.deltaTime;
        }
        transform.position = currentPosition;
    }
}
