using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryCards : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    GameObject _newTooltip, _newTooltipBckg;
    bool hoverBool;
    Button cardButton;
    string cardTooltip;

    void Awake()
    {
        cardButton = GetComponent<Button>();
        cardTooltip = name;
    }

    void Update()
    {
        if (hoverBool) OnPointerOver();
    }

    public void OnPointerEnter(PointerEventData data)
    {
        hoverBool = true;
    }

    public void OnPointerExit(PointerEventData data)
    {
        hoverBool = false;
        TooltipManager.Instance.DeactivateTooltip(cardButton, _newTooltip, _newTooltipBckg, cardTooltip);
    }

    public void OnPointerUp(PointerEventData data)
    {
        hoverBool = false;
        TooltipManager.Instance.DeactivateTooltip(cardButton, _newTooltip, _newTooltipBckg, cardTooltip);
    }


    void OnPointerOver()
    {
        TooltipManager.Instance.OnHoverButtonActivateTooltip(cardButton, _newTooltip, _newTooltipBckg, cardTooltip);
    }
}
