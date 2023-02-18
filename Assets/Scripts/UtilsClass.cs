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
    
        foreach (Transform t in targets) 
        {
            float distanceToTarget = Vector3.Distance(t.position, transform.position);
            if (distanceToTarget < closestDistance) {
                closestTarget = t;
                closestDistance = distanceToTarget;
            }
        }
    
        return closestTarget;
    }
}
