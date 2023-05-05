using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    public static PlayerManager Instance { get; private set; }

    [SerializeField] GameObject[] playerPrefabs;

    void Awake () {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        ReplacePlayer(playerPrefabs[PlayerPrefs.GetInt(PlayerPrefsKeys.PLAYER)]);
    }

    public void ReplacePlayer(GameObject replaceBy) {
        if (Player.Instance != null) {
            Destroy(Player.Instance.gameObject);
        }
        GameObject newPlayer = Instantiate(replaceBy, new Vector3(0, -10, 0), Quaternion.identity);
        PlayerPrefs.SetInt(PlayerPrefsKeys.PLAYER, (int)newPlayer.GetComponent<Player>().playerType);
    }
}
