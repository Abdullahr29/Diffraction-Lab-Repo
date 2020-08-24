using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LabScriptTool : Tool
{
    GameObject _activeBckg;

    GameObject _newTooltip, _newTooltipBckg;
    bool hoverBool;
    Button labScriptButton;
    string labScriptTooltip = "Lab Script Tool";
    
    void OnEnable()
    {
        labScriptButton = this.GetComponent<Button>();
    }
    
    public override void ButtonInteract()
    {
        TooltrayController.Instance.ActiveStaticToolBckg(_activeBckg, 0, true);
        ModalManager.Instance.ActivateLabScript();
    }

    public override void OnPointerEnter(PointerEventData data)
    {
        TooltipManager.Instance.OnHoverButtonActivateTooltip(labScriptButton, _newTooltip, _newTooltipBckg, labScriptTooltip);
    }

    public override void OnPointerExit(PointerEventData data)
    {
        TooltipManager.Instance.DeactivateTooltip(labScriptButton, _newTooltip, _newTooltipBckg, labScriptTooltip);
    }

}
