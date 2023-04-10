using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour {
    [SerializeField] private Sprite[] treeSprites;
    [SerializeField] private Sprite[] darkTreeSprites;
    public bool isDarked;

    void Awake() {
        GetComponent<SpriteRenderer>().sprite = treeSprites[Random.Range(0, treeSprites.Length)];
    }

    public IEnumerator HideTreeTemporary(GameObject gameObject, float waitForSeconds) {
        gameObject.GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(waitForSeconds);
        if (gameObject != null) gameObject.GetComponent<Collider2D>().enabled = true;
    }

    public void SwitchToDarkTree() {
        if (!isDarked) {
            isDarked = true;
            GetComponent<SpriteRenderer>().sprite = darkTreeSprites[Random.Range(0, darkTreeSprites.Length)];
        }
    }
}
