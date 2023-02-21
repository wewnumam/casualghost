using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour {
    [SerializeField] Skill skillType;

    void Awake() {
        GetComponent<Button>().onClick.AddListener(() => GamePanelManager.Instance.NextLevel());

        if (skillType == Skill.SPEED_UP) {
            GetComponent<Button>().onClick.AddListener(() => SkillEnhancer.Instance.SpeedUp());
        } else if (skillType == Skill.FAST_RELOAD) {
            GetComponent<Button>().onClick.AddListener(() => SkillEnhancer.Instance.FastReload());
        } else if (skillType == Skill.FAST_TRIGGER) {
            GetComponent<Button>().onClick.AddListener(() => SkillEnhancer.Instance.FastTrigger());
        }
    }
}

public enum Skill {
    SPEED_UP,
    FAST_RELOAD,
    FAST_TRIGGER
}
