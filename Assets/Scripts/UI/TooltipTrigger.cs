using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    [Header("Tooltip Properties")]
    public string header;
    public string content;

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
