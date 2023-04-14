using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealArea : MonoBehaviour {
    [SerializeField] private float healAmount;
    [SerializeField] private float waitForSeconds;
    private bool canActive = true;
    private List<HealthSystem> healthSystems;

    void Start() {
        healthSystems = new List<HealthSystem>();
        healthSystems.Add(GameObject.FindGameObjectWithTag(Tags.PLAYER).GetComponent<HealthSystem>());
        SoundManager.Instance.PlaySound(EnumsManager.SoundEffect.PLAYER_SKILL_HEAL_AREA);
    }

    void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.CompareTag(Tags.PLAYER) ||
            collision.gameObject.CompareTag(Tags.BANYAN) ||
            collision.gameObject.CompareTag(Tags.ROOT) ||
            collision.gameObject.CompareTag(Tags.CANNON) ||
            collision.gameObject.CompareTag(Tags.DECOY)) {

            healthSystems.Add(collision.gameObject.GetComponent<HealthSystem>());
            
            if (canActive) {
                StartCoroutine(Heal(healthSystems));
            }
        }
    }

    void OnDestroy() {
        SoundManager.Instance.StopSound(EnumsManager.SoundEffect.PLAYER_SKILL_HEAL_AREA);
    }

    IEnumerator Heal(List<HealthSystem> healthSystems) {
        canActive = false;
        yield return new WaitForSeconds(waitForSeconds);
        canActive = true;

        List<HealthSystem> filteredHealthSystems = new List<HealthSystem>();
        foreach (HealthSystem value in healthSystems) {
            if (!filteredHealthSystems.Contains(value)) {
                filteredHealthSystems.Add(value);
            }
        }

        foreach (var hs in filteredHealthSystems) {
            hs.GetComponent<FloatingText>().InstantiateFloatingText((healAmount * 100).ToString(), hs.transform, new Color(0f, 1f, 0f, 1f));
            hs.Heal(healAmount);
            if ((hs.gameObject.CompareTag(Tags.PLAYER) && hs.IsDying()) || (hs.gameObject.CompareTag(Tags.BANYAN) && hs.IsDying())) {
                PostProcessingEffect.Instance.DyingEffect(healAmount / 10, true);
            }
        }
    }
}
