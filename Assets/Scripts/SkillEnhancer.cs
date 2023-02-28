using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEnhancer : MonoBehaviour {
    public static SkillEnhancer Instance { get; private set; }

    [SerializeField] private float speedAddBy;
    [SerializeField] private float reloadTimeDivideBy;
    [SerializeField] private float pullTriggerTimeDivideBy;
    [SerializeField] private float coinCollectionRadiusAddBy;
    [SerializeField] private float maxHealthAddBy;
    [SerializeField] private float bulletDamageAddBy;
    [SerializeField] List<GameObject> skills;
    [SerializeField] Transform skillParent;

    void Awake () {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void InstantiateRandomSkill() {
        foreach (Transform child in skillParent.transform) {
            Destroy(child.gameObject);
        }

        Instantiate(skills[Random.Range(0, skills.Count)], skillParent);
        Instantiate(skills[Random.Range(0, skills.Count)], skillParent);
        Instantiate(skills[Random.Range(0, skills.Count)], skillParent);
    }

    public void SpeedUp() {
        GameObject player = GameObject.FindGameObjectWithTag(Tags.PLAYER);

        player.GetComponent<PlayerMovement>().normalSpeed += speedAddBy;
        player.GetComponent<PlayerMovement>().SetCurrentSpeed(player.GetComponent<PlayerMovement>().normalSpeed);
        player.GetComponent<PlayerMovement>().boostSpeed = player.GetComponent<PlayerMovement>().normalSpeed * 2;
    }

    public void FastReload() {
        GameObject player = GameObject.FindGameObjectWithTag(Tags.PLAYER);

        player.GetComponentInChildren<PlayerShooting>().reloadTime /= reloadTimeDivideBy;
        player.GetComponent<Animator>().SetFloat(AnimationTags.PLAYER_RELOAD_TIME, 1 + reloadTimeDivideBy); 
    }

    public void FastTrigger() {
        GameObject player = GameObject.FindGameObjectWithTag(Tags.PLAYER);

        player.GetComponentInChildren<PlayerShooting>().pullTriggerTime /= pullTriggerTimeDivideBy;
    }

    public void ExpandCoinCollectionArea() {
        GameObject player = GameObject.FindGameObjectWithTag(Tags.PLAYER);

        player.GetComponent<CircleCollider2D>().radius += coinCollectionRadiusAddBy; 
    }

    public void IncreaseMaxHealth() {
        GameObject player = GameObject.FindGameObjectWithTag(Tags.PLAYER);

        player.GetComponent<HealthSystem>().SetMaxHealth(player.GetComponent<HealthSystem>().maxHealth + maxHealthAddBy);
        player.GetComponent<HealthSystem>().SetCurrentHealth(GetComponent<HealthSystem>().maxHealth);
    }

    public void IncreaseBulletDamage() {
        GameObject player = GameObject.FindGameObjectWithTag(Tags.PLAYER);

        player.GetComponentInChildren<PlayerShooting>().bulletDamage += bulletDamageAddBy;
    }
}
