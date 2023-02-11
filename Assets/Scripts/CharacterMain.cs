using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterMain : MonoBehaviour
{
    [Header("Player Properties")]
	public float health = 5f;
    private bool canAttacked = true;

    void Update() {
        if (health == 0) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnCollisionStay2D(Collision2D collision) {
		if (collision.gameObject.CompareTag(Tags.ENEMY) && canAttacked) {
            GetComponent<Animator>().Play(AnimationTags.PLAYER_TRANCE);
			StartCoroutine(Attacked());
		}
	}

	IEnumerator Attacked() {
        canAttacked = false;
        yield return new WaitForSeconds(1f);
		health--;
        canAttacked = true;
	}
}
