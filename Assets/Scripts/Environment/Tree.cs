using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour {
    [Header("Caching Components")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private new Collider2D collider2D;

    [SerializeField] private TreeSprites[] treeSprites;
    public int treeSpritesIndex;

    void Awake() {
        treeSpritesIndex = Random.Range(0, treeSprites.Length);
        spriteRenderer.sprite = treeSprites[treeSpritesIndex].treeSprites;
    }

    public IEnumerator HideTreeTemporary(float waitForSeconds) {
        collider2D.enabled = false;
        yield return new WaitForSeconds(waitForSeconds);
        collider2D.enabled = true;
    }

    public void SwitchToDarkTree() {
        if (!treeSprites[treeSpritesIndex].isDarked) {
            treeSprites[treeSpritesIndex].isDarked = true;
            spriteRenderer.sprite = treeSprites[treeSpritesIndex].darkTreeSprites;
        }
    }
}

[System.Serializable]
public class TreeSprites {
    public Sprite treeSprites;
    public Sprite darkTreeSprites;
    public bool isDarked;
}
