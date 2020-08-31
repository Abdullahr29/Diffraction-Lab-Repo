using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvestigateTool : Tool
{

    // Active tool sprite not enabled yet otherwise it would cause bugs.
    //Only add when script for tool is written

    private GameObject _activeBckg;
    string activeName = "Investigate Active";

    GameObject _newTooltip, _newTooltipBckg;
    bool hoverBool;
    Button investigateButton;
    string investigateTooltip = "Investigate Tool";

    void OnEnable()
    {
        gameObject.GetComponent<Image>().sprite = TooltrayController.Instance._investigateSprite;
        investigateButton = this.GetComponent<Button>();
    }

    void Update()
    {
        if (hoverBool) this.OnPointerOver();
    }

    public override void ButtonInteract()
    {
        //TooltrayController.Instance.ActiveToolBckg(_activeBckg, 2, true, activeName);
    }
    
    public override void DeactivateButton()
    {
        //TooltrayController.Instance.ActiveToolBckg(_activeBckg, 2, false, activeName);
    }

    public override void OnPointerEnter(PointerEventData data)
    {
        hoverBool = true;
    }

    public override void OnPointerExit(PointerEventData data)
    {
        hoverBool = false;
        TooltipManager.Instance.DeactivateTooltip(investigateButton, _newTooltip, _newTooltipBckg, investigateTooltip);
    }

    void OnPointerOver()
    {
        TooltipManager.Instance.OnHoverButtonActivateTooltip(investigateButton, _newTooltip, _newTooltipBckg, investigateTooltip);
    }
}