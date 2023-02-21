using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEnhancer : MonoBehaviour {
    public static SkillEnhancer Instance { get; private set; }

    [SerializeField] private float speedMultiplyBy;
    [SerializeField] private float reloadTimeDivideBy;
    [SerializeField] private float pullTriggerTimeDivideBy;
    [SerializeField] List<GameObject> skills;
    [SerializeField] Transform skillParent;

    void Awake () {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        Instantiate(skills[Random.Range(0, skills.Count)], skillParent);
        Instantiate(skills[Random.Range(0, skills.Count)], skillParent);
        Instantiate(skills[Random.Range(0, skills.Count)], skillParent);
    }

    public void SpeedUp() {
        GameObject player = GameObject.FindGameObjectWithTag(Tags.PLAYER);

        player.GetComponent<PlayerMovement>().normalSpeed *= speedMultiplyBy;
        player.GetComponent<PlayerMovement>().boostSpeed *= speedMultiplyBy;
    }

    public void FastReload() {
        GameObject player = GameObject.FindGameObjectWithTag(Tags.PLAYER);

        player.GetComponentInChildren<PlayerShooting>().reloadTime /= reloadTimeDivideBy;
        player.GetComponent<Animator>().SetFloat("reloadTime", 1 + reloadTimeDivideBy); 
    }

    public void FastTrigger() {
        GameObject player = GameObject.FindGameObjectWithTag(Tags.PLAYER);

        player.GetComponentInChildren<PlayerShooting>().pullTriggerTime /= pullTriggerTimeDivideBy;
    }
}
