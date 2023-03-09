using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    [Header("Tooltip Properties")]
    [SerializeField] private string header;
    [SerializeField] private string content;

    public void OnPointerEnter(PointerEventData eventData) {
        TooltipSystem.Show(content, header);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData) {
        TooltipSystem.Hide();
    }

    void IPointerClickHandler.OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData) {
        TooltipSystem.Hide();
    }
}
