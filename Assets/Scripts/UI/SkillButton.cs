using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour {
    [SerializeField] Skill skillType;

    private enum Skill {
        SPEED_UP,
        FAST_RELOAD,
        FAST_TRIGGER,
        EXPAND_COIN_COLLECTION_AREA,
        INCREASE_MAX_HEALTH,
        INCREASE_BULLET_DAMAGE
    }

    void Awake() {
        GetComponent<Button>().onClick.AddListener(() => GamePanelManager.Instance.NextLevel());

        switch (skillType)
        {
            case Skill.SPEED_UP:
                GetComponent<Button>().onClick.AddListener(() => SkillEnhancer.Instance.SpeedUp());
                break;
            case Skill.FAST_RELOAD:
                GetComponent<Button>().onClick.AddListener(() => SkillEnhancer.Instance.FastReload());
                break;
            case Skill.FAST_TRIGGER:
                GetComponent<Button>().onClick.AddListener(() => SkillEnhancer.Instance.FastTrigger());
                break;
            case Skill.EXPAND_COIN_COLLECTION_AREA:
                GetComponent<Button>().onClick.AddListener(() => SkillEnhancer.Instance.ExpandCoinCollectionArea());
                break;
            case Skill.INCREASE_MAX_HEALTH:
                GetComponent<Button>().onClick.AddListener(() => SkillEnhancer.Instance.IncreaseMaxHealth());
                break;
            case Skill.INCREASE_BULLET_DAMAGE:
                GetComponent<Button>().onClick.AddListener(() => SkillEnhancer.Instance.IncreaseBulletDamage());
                break;
        }
    }
}


