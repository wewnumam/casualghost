using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleItem : MonoBehaviour {
    public static CollectibleItem Instance { get; private set; }

    public Sprite branchSprite;
    public Material branchMaterial;
    [SerializeField] private Item[] items;
    [HideInInspector] public Item currentItem;
    [SerializeField] private int enemyKillAmountToSpawnCollectibleItem;
    private int enemyKillCounter;
    public GameObject collectibleItemObject;
    [HideInInspector] public bool canSpawnCollectibleItem;
    [HideInInspector] public bool canActivateSkill;
    private float currentTime;
    private float maxTime;
    [SerializeField] private Slider progressBar;

    void Awake () {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Update() {
        if (GameObject.FindGameObjectWithTag(Tags.PLAYER) == null) return;

        progressBar = GameObject.FindGameObjectWithTag(Tags.COLLECTIBLE_ITEM_BAR).GetComponent<Slider>();
            
        if (currentTime <= 0) {
            currentTime = 0;
            progressBar.value = 0;
        } else {
            currentTime -= Time.deltaTime;
            progressBar.maxValue = maxTime;
            progressBar.value = currentTime;
        }
    }

    public void AddEnemyKillCounter() {
        enemyKillCounter++;
        if (enemyKillCounter >= enemyKillAmountToSpawnCollectibleItem) {
            currentItem = items[Random.Range(0, items.Length)];
            canSpawnCollectibleItem = true;
            enemyKillCounter = 0;
        } else {
            canSpawnCollectibleItem = false;
        }
    }

    public void ActivateSkill(Item item) {
        maxTime = item.activateTime;
        currentTime = item.activateTime;

        GameObject skill = Instantiate(item.skillObject, GameObject.FindGameObjectWithTag(Tags.PLAYER).transform);
        Destroy(skill, item.activateTime);
        SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.TUTORIAL_DONE);
    }
}


[System.Serializable]
public class Item {
    public EnumsManager.Item itemType;
    public Sprite icon;
    public Material material;
    public float activateTime;
    public GameObject skillObject;
}