using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LabScriptTool : Tool
{
    private GameObject _activeBckg;
    GameObject _newTooltip;
    GameObject _newTooltipBckg;
    bool hoverBool;
    Button labScriptButton;
    /*
    void OnEnable()
    {
        labScriptButton = this.GetComponent<Button>();
        TooltipManager.Instance.addNewButton(labScriptButton, "Lab Script");
    }

    void Update()
    {
        TooltipManager.Instance.onHoverButtonCreateTooltip(labScriptButton, _newTooltip, _newTooltipBckg, hoverBool);
    }
    */
    
    public override void ButtonInteract()
    {
        TooltrayController.Instance.ActiveStaticToolBckg(_activeBckg, 0, true);
        ModalManager.Instance.ActivateLabScript();
    }
}
