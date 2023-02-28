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

        if (skillType == Skill.SPEED_UP) {
            GetComponent<Button>().onClick.AddListener(() => SkillEnhancer.Instance.SpeedUp());
        } else if (skillType == Skill.FAST_RELOAD) {
            GetComponent<Button>().onClick.AddListener(() => SkillEnhancer.Instance.FastReload());
        } else if (skillType == Skill.FAST_TRIGGER) {
            GetComponent<Button>().onClick.AddListener(() => SkillEnhancer.Instance.FastTrigger());
        } else if (skillType == Skill.EXPAND_COIN_COLLECTION_AREA) {
            GetComponent<Button>().onClick.AddListener(() => SkillEnhancer.Instance.ExpandCoinCollectionArea());
        } else if (skillType == Skill.INCREASE_MAX_HEALTH) {
            GetComponent<Button>().onClick.AddListener(() => SkillEnhancer.Instance.IncreaseMaxHealth());
        } else if (skillType == Skill.INCREASE_BULLET_DAMAGE) {
            GetComponent<Button>().onClick.AddListener(() => SkillEnhancer.Instance.IncreaseBulletDamage());
        }
    }
}


