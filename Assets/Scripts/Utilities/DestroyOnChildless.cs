using UnityEngine;

/// Utility class to destroy any GO that has all the children killed or childess
/// This would be useful like when a GO with an auto-destroy particle as child:
/// To destroy the parent GO when the PS is killed.
public class DestroyOnChildless : MonoBehaviour {
    private void Update() {
        if (transform.childCount == 0) {
            Destroy(gameObject);
        }
    }
}