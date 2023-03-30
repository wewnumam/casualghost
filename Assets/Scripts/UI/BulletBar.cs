using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletBar : MonoBehaviour {
    public int maxRound;
    public int roundsLeft;

    [SerializeField] private Image[] bullets;
    [SerializeField] private Sprite fullBullet;
    [SerializeField] private Sprite emptyBullet;

    void Update() {
        maxRound = GetComponent<PlayerShooting>().maxRound;
        roundsLeft = GetComponent<PlayerShooting>().roundsLeft;

        for (int i = 0; i < bullets.Length; i++) {
            if (i < roundsLeft) {
                bullets[i].sprite = fullBullet;
            } else {
                bullets[i].sprite = emptyBullet;
            }

            if (i < maxRound) {
                bullets[i].enabled = true;
            } else {
                bullets[i].enabled = false;
            }
        }
    }

}
