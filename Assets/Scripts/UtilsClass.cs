using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilsClass {
    public static Vector3 GetMouseWorldPosition() {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }
    public static Vector3 GetMouseWorldPositionWithZ() {
        return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    }
    public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera) {
        return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    }
    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera) {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

    public static float GetAngleFromVectorFloat(Vector3 dir) {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }
    
    public static Transform FindClosestTransform(Transform transform, Transform[] targets) {
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;
    
        foreach (Transform t in targets)  {
            float distanceToTarget = Vector3.Distance(t.position, transform.position);
            if (distanceToTarget < closestDistance) {
                closestTarget = t;
                closestDistance = distanceToTarget;
            }
        }
    
        return closestTarget;
    }

    public static void AimRotation(Transform transform, Vector3 positionToAim) {
		// Rotation facing towards mouse cursor
		Vector3 aimDirection = (positionToAim - transform.position).normalized;
		float angle = UtilsClass.GetAngleFromVectorFloat(aimDirection);
		transform.eulerAngles = new Vector3(0, 0, angle);

		// Flip weapon vertically
		Vector3 localScale = transform.localScale;
		if (angle > 90 && angle < 270) {
			localScale.y = -1f;
		} else {
			localScale.y = +1f;
		}
		transform.localScale = localScale;
	}

    public static Transform[] GetGameObjectsTransform(GameObject[] gameObjects, int indexOffset = 0) {
        Transform[] gameObjectsTransfrom = new Transform[gameObjects.Length + indexOffset];

        for (int i = 0; i < gameObjects.Length; i++) {
            gameObjectsTransfrom[i] = gameObjects[i].transform;
        }

        return gameObjectsTransfrom;
    }
}
