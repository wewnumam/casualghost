using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour {
    [SerializeField] private TreeSprites[] treeSprites;
    public int treeSpritesIndex;

    void Awake() {
        treeSpritesIndex = Random.Range(0, treeSprites.Length);
        GetComponent<SpriteRenderer>().sprite = treeSprites[treeSpritesIndex].treeSprites;
    }

    public IEnumerator HideTreeTemporary(GameObject gameObject, float waitForSeconds) {
        gameObject.GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(waitForSeconds);
        if (gameObject != null) gameObject.GetComponent<Collider2D>().enabled = true;
    }

    public void SwitchToDarkTree() {
        if (!treeSprites[treeSpritesIndex].isDarked) {
            treeSprites[treeSpritesIndex].isDarked = true;
            GetComponent<SpriteRenderer>().sprite = treeSprites[treeSpritesIndex].darkTreeSprites;
        }
    }
}

[System.Serializable]
public class TreeSprites {
    public Sprite treeSprites;
    public Sprite darkTreeSprites;
    public bool isDarked;
}
