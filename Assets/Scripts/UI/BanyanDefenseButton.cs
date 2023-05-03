using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BanyanDefenseButton : MonoBehaviour {
    public void InstantiatePrefab(GameObject banyanDefenseObjectPrefab) {
        GameObject banyanDefenseObject = Instantiate(banyanDefenseObjectPrefab, BanyanDefenseManager.Instance.parentBanyanDefenseObject);

        int sameDefenseTypeCounter = 0;
        foreach (Transform child in BanyanDefenseManager.Instance.parentBanyanDefenseObject) {
            if (child.gameObject.GetComponent<BanyanDefenseObject>() != null &&
                banyanDefenseObject.GetComponent<BanyanDefenseObject>().defenseType == child.gameObject.GetComponent<BanyanDefenseObject>().defenseType) {
                sameDefenseTypeCounter++;
            }
        }

        if (sameDefenseTypeCounter % 2 == 0) {
		    banyanDefenseObject.transform.Rotate(new Vector3(0, 0, 10 * -Mathf.FloorToInt(sameDefenseTypeCounter / 2)));
        } else {
            banyanDefenseObject.transform.Rotate(new Vector3(0, 0, 10 * Mathf.FloorToInt(sameDefenseTypeCounter / 2)));
        }

        foreach (Transform child in banyanDefenseObject.transform) {
            child.rotation = Quaternion.identity;
        }

        BanyanDefenseManager.Instance.buttonCanvas.SetActive(false);
    }
}
