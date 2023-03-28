using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour {
    [SerializeField] private GameObject floatingTextPrefab;
    [SerializeField] private Color defaultColor;
    [SerializeField] private float offsetY;

    public void InstantiateFloatingText(string text, Transform parent) {
        GameObject ft = Instantiate(floatingTextPrefab, parent);
        ft.GetComponentInChildren<TMP_Text>().text = text;
        ft.GetComponentInChildren<TMP_Text>().color = defaultColor;

        Vector3 currentPosition = ft.transform.position;
        currentPosition.x += Random.Range(-1f, 1f);
        currentPosition.y += offsetY;
        ft.transform.position = currentPosition;

        if (parent.localScale.x < 0) {
            Vector3 localScale = Vector3.one;
            localScale.x *= -1f;
            ft.transform.localScale = localScale;
        }

        Destroy(ft, 1f);
    }

    public void InstantiateFloatingText(string text, Transform parent, Color overrideColor) {
        GameObject ft = Instantiate(floatingTextPrefab, parent);
        ft.GetComponentInChildren<TMP_Text>().text = text;

        Vector3 currentPosition = ft.transform.position;
        currentPosition.x += Random.Range(-1f, 1f);
        currentPosition.y += offsetY;
        ft.transform.position = currentPosition;

        if (overrideColor == null) {
            ft.GetComponentInChildren<TMP_Text>().color = defaultColor;
        } else {
            ft.GetComponentInChildren<TMP_Text>().color = overrideColor;
        }

        if (parent.transform.localScale.x < 0) {
            Vector3 localScale = Vector3.one;
            localScale.x *= -1f;
            ft.transform.localScale = localScale;
        }

        Destroy(ft, 1f);
    }
}
