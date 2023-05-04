using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoEffect : MonoBehaviour {
    private float timeBetweenSpawns;
    [SerializeField] float startTimeBetweenSpawns;
    [SerializeField] GameObject echo;

    void Update() {
        if (GetComponent<PlayerMovement>().movement == Vector2.zero) return;

        if (timeBetweenSpawns <= 0) {
            GameObject instance = Instantiate(echo, transform.position, Quaternion.identity);
            float angle = UtilsClass.GetAngleFromVectorFloat((UtilsClass.GetMouseWorldPosition(GameManager.Instance.mainCamera) - instance.transform.position).normalized);
            Vector3 localScale = Vector3.one;
            if (angle > 90 && angle < 270) {
                localScale.x *= -1f;
            } else {
                localScale.x *= +1f;
            }
            instance.transform.localScale = localScale;
            instance.GetComponent<Animator>().Play(AnimationTags.PLAYER_ECHO);
            Destroy(instance, 2f);

            timeBetweenSpawns = startTimeBetweenSpawns;
        } else {
            timeBetweenSpawns -= Time.deltaTime;
        }
    }
}
