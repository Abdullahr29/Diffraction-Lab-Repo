using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HelpTool : Tool
{
    private GameObject _activeBckg;
    GameObject _newTooltip;
    GameObject _newTooltipBckg;
    bool hoverBool;
    Button helpButton;
    /*
    void OnEnable()
    {
        helpButton = this.GetComponent<Button>();
        TooltipManager.Instance.addNewButton(helpButton, "Help Tool");
    }

    void Update()
    {
        TooltipManager.Instance.onHoverButtonCreateTooltip(helpButton, _newTooltip, _newTooltipBckg, hoverBool);
    }*/

    public override void ButtonInteract()
    {
        TooltrayController.Instance.ActiveStaticToolBckg(_activeBckg, 1, true);
        ModalManager.Instance.ActivateHelpModal(UIController.Instance.currentMode);
    }
}