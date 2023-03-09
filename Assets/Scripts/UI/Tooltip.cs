using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI headerField;
    [SerializeField] private TextMeshProUGUI contentField;
    [SerializeField] private LayoutElement layoutElement;
    [SerializeField] private int characterWrapLimit;
    private RectTransform rectTransform;

    void Awake() {
        rectTransform = GetComponent<RectTransform>();    
    }

    public void SetText(string content, string header = "") {
        if (string.IsNullOrEmpty(header)) {
            headerField.gameObject.SetActive(false);
        } else {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }

        contentField.text = content;

        int headerLength = headerField.text.Length;    
        int ContentLength = contentField.text.Length;

        layoutElement.enabled = (headerLength > characterWrapLimit || ContentLength > characterWrapLimit) ? true : false; 
    }

    void Update() {
        if (Application.isEditor) {
            int headerLength = headerField.text.Length;    
            int ContentLength = contentField.text.Length;

            layoutElement.enabled = (headerLength > characterWrapLimit || ContentLength > characterWrapLimit) ? true : false;    
        }

        float pivotX = UtilsClass.GetMouseWorldPosition().x / Screen.width;
        float pivotY = UtilsClass.GetMouseWorldPosition().y / Screen.height;

        rectTransform.pivot = new Vector2(pivotX, pivotY);
        transform.position = UtilsClass.GetMouseWorldPosition();
    }
    
}
