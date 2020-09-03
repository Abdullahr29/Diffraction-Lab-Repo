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
    
    void Awake()
    {
        labScriptButton = this.GetComponent<Button>();
    }

    void Update()
    {
        if (hoverBool) this.OnPointerOver();
    }

    public override void ButtonInteract()
    {
        TooltrayController.Instance.ActiveStaticToolBckg(_activeBckg, 0, true);
        ModalManager.Instance.ActivateLabScript();
        DeactivateInputManager();
    }

    public override void OnPointerEnter(PointerEventData data)
    {
        hoverBool = true;
    }

    public override void OnPointerExit(PointerEventData data)
    {
        hoverBool = false;
        TooltipManager.Instance.DeactivateTooltip(labScriptButton, _newTooltip, _newTooltipBckg, labScriptTooltip);
    }

    void OnPointerOver()
    {
        TooltipManager.Instance.OnHoverButtonActivateTooltip(labScriptButton, _newTooltip, _newTooltipBckg, labScriptTooltip);
    }

}
