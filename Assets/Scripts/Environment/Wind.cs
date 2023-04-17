using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour {
    [SerializeField] private float moveSpeed;

    private void Start() {
        if (GetComponent<AreaEffector2D>().forceMagnitude < 0) {
            GetComponent<SpriteRenderer>().flipX = true;
            Vector3 currentLocalScale = GetComponentInChildren<ParticleSystem>().transform.localScale;
            currentLocalScale.x *= -1; 
            GetComponentInChildren<ParticleSystem>().transform.localScale = currentLocalScale;
        }
    }

    void Update() {
        Vector3 currentPosition = transform.position;
        if (GetComponent<AreaEffector2D>().forceMagnitude > 0) {
            currentPosition.x += moveSpeed * Time.deltaTime;
        } else {
            currentPosition.x -= moveSpeed * Time.deltaTime;
        }
        transform.position = currentPosition;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.CompareTag(Tags.PLAYER)) {
            SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.WIND);
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.CompareTag(Tags.PLAYER)) {
            SoundManager.Instance.StopSound(EnumsManager.SoundEffect.WIND);
        }
    }
}
