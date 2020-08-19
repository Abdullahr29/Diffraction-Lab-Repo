using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AngleMeasTool : Tool
{

    // Active tool sprite not enabled yet otherwise it would cause bugs.
    //Only add when script for tool is written

    private GameObject _activeBckg;
    string activeName = "Plane Orientation Active";

    GameObject _newTooltip, _newTooltipBckg;
    bool hoverBool;
    Button angleButton;
    string angleTooltip = "Plane Orientation Tool";
    void OnEnable()
    {
        gameObject.GetComponent<Image>().sprite = TooltrayController.Instance._angleSprite;
        angleButton = this.GetComponent<Button>();
    }

    public override void ButtonInteract()
    {
        //TooltrayController.Instance.ActiveToolBckg(_activeBckg, 1, true, activeName);
    }
    
    public override void DeactivateButton()
    {
        //TooltrayController.Instance.ActiveToolBckg(_activeBckg, 1, false, activeName);
    }

    public override void OnPointerEnter(PointerEventData data)
    {
        TooltipManager.Instance.OnHoverButtonActivateTooltip(angleButton, _newTooltip, _newTooltipBckg, angleTooltip);
    }

    public override void OnPointerExit(PointerEventData data)
    {
        TooltipManager.Instance.DeactivateTooltip(angleButton, _newTooltip, _newTooltipBckg, angleTooltip);
    }
}
