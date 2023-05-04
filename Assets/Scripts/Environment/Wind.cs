using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour {
    [SerializeField] private float moveSpeed;
    
    [Header("Caching Components")]
    [SerializeField] private AreaEffector2D areaEffector2D;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private new ParticleSystem particleSystem;
    private Vector3 currentPosition;
    private Vector3 currentLocalScale;

    private void Awake() {
        if (areaEffector2D.forceMagnitude < 0) {
            spriteRenderer.flipX = true;
            currentLocalScale = particleSystem.transform.localScale;
            currentLocalScale.x *= -1; 
            particleSystem.transform.localScale = currentLocalScale;
        }
    }

    void Update() {
        currentPosition = transform.position;
        if (areaEffector2D.forceMagnitude > 0) {
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
