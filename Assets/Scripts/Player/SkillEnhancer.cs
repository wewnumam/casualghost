using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillEnhancer : MonoBehaviour {
    public static SkillEnhancer Instance { get; private set; }

    [Header("Skill Button Instantiate Properties")]
    [SerializeField] List<GameObject> skills;
    [SerializeField] Transform skillParent;
    [SerializeField] Transform skillList;

    [Header("Enhancer Properties")]
    [SerializeField] private float speedAddBy;
    [SerializeField] private float reloadTimeDivideBy;
    [SerializeField] private float pullTriggerTimeDivideBy;
    [SerializeField] private float coinCollectionRadiusAddBy;
    [SerializeField] private float maxHealthAddBy;
    [SerializeField] private float bulletDamageAddBy;
    [SerializeField] private float sprintDurationAddBy;

    void Awake () {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void AddSkillInfoToList(Image skillImage, TooltipTrigger tooltipTrigger) {
        GameObject skill = Instantiate(skillImage.gameObject, skillList);
        skill.AddComponent<BoxCollider2D>();
        skill.GetComponent<BoxCollider2D>().isTrigger = true;
        Destroy(skill.GetComponent<SkillButton>());
    }

    public void InstantiateRandomSkill() {
        foreach (Transform child in skillParent.transform) {
            Destroy(child.gameObject);
        }

        // Shuffle the list to ensure randomness
        for (int i = skills.Count - 1; i > 0; i--) {
            int j = UnityEngine.Random.Range(0, i + 1);
            GameObject temp = skills[i];
            skills[i] = skills[j];
            skills[j] = temp;
        }

        // Get the first 3 values from the shuffled list
        for (int i = 0; i < 3; i++) {
            Instantiate(skills[i], skillParent);
        }
    }

    public void SpeedUp() {
        PlayerMovement playerMovement = GameObject.FindGameObjectWithTag(Tags.PLAYER).GetComponent<PlayerMovement>();

        playerMovement.SpeedUp(speedAddBy);
    }

    public void FastReload() {
        PlayerShooting playerShooting = GameObject.FindGameObjectWithTag(Tags.PLAYER).GetComponentInChildren<PlayerShooting>();
        
        playerShooting.FastReload(reloadTimeDivideBy);
    }

    public void FastTrigger() {
        PlayerShooting playerShooting = GameObject.FindGameObjectWithTag(Tags.PLAYER).GetComponentInChildren<PlayerShooting>();

        playerShooting.FastTrigger(pullTriggerTimeDivideBy);
    }

    public void ExpandCoinCollectionArea() {
        CircleCollider2D playerCollider = GameObject.FindGameObjectWithTag(Tags.PLAYER).GetComponent<CircleCollider2D>();

        playerCollider.radius += coinCollectionRadiusAddBy; 
    }

    public void IncreaseMaxHealth() {
        HealthSystem playerHealthSystem = GameObject.FindGameObjectWithTag(Tags.PLAYER).GetComponent<HealthSystem>();

        playerHealthSystem.SetMaxHealth(playerHealthSystem.maxHealth + maxHealthAddBy);
        playerHealthSystem.SetCurrentHealth(playerHealthSystem.maxHealth);
    }

    public void IncreaseBulletDamage() {
        PlayerShooting playerShooting = GameObject.FindGameObjectWithTag(Tags.PLAYER).GetComponentInChildren<PlayerShooting>();

        playerShooting.IncreaseBulletDamage(bulletDamageAddBy);
    }

    public void BreathRoom() {
        Player.Instance.SetBreathRoomActive();
    }

    public void IncreaseSprintDuration() {
        PlayerMovement playerMovement = GameObject.FindGameObjectWithTag(Tags.PLAYER).GetComponent<PlayerMovement>();

        playerMovement.IncreaseSprintDuration(sprintDurationAddBy);
    }
}
