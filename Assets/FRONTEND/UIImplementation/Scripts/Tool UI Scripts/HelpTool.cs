using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HelpTool : Tool
{
    private GameObject _activeBckg;

    GameObject _newTooltip, _newTooltipBckg;
    bool hoverBool;
    Button helpButton;
    string helpTooltip = "Help Tool";
    void OnEnable()
    {
        helpButton = this.GetComponent<Button>();
    }

    public override void ButtonInteract()
    {
        TooltrayController.Instance.ActiveStaticToolBckg(_activeBckg, 1, true);
        ModalManager.Instance.ActivateHelpModal(UIController.Instance.currentMode);
    }

    public override void OnPointerEnter(PointerEventData data)
    {
        TooltipManager.Instance.OnHoverButtonActivateTooltip(helpButton, _newTooltip, _newTooltipBckg, helpTooltip);
    }

    public override void OnPointerExit(PointerEventData data)
    {
        TooltipManager.Instance.DeactivateTooltip(helpButton, _newTooltip, _newTooltipBckg, helpTooltip);
    }
}