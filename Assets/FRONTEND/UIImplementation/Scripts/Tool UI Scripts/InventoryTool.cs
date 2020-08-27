using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryTool : Tool
{
    private GameObject _activeBckg;
    string activeName = "Inventory Active";

    GameObject _newTooltip, _newTooltipBckg;
    bool hoverBool;
    Button inventoryButton;
    string inventoryTooltip = "Inventory";

    void OnEnable()
    {
        gameObject.GetComponent<Image>().sprite = TooltrayController.Instance._inventorySprite;
        inventoryButton = this.GetComponent<Button>();
    }

    void Update()
    {
        if (hoverBool) this.OnPointerOver();
    }

    bool isBeingUsed = false;
    public override void ButtonInteract()
    {
        isBeingUsed = !isBeingUsed;

        if (isBeingUsed)
        {
            TooltrayController.Instance.ActiveToolBckg(_activeBckg, 0, true, activeName);
            ModalManager.Instance.ActivateInventory();

            TooltrayController.Instance.newTool = this;
            TooltrayController.Instance.SwitchTool();
            TooltrayController.Instance.activeTools.Add(this);


            DeactivateInputManager();
        }
    }

    public override void OnPointerEnter(PointerEventData data)
    {
        hoverBool = true;
    }

    public override void OnPointerExit(PointerEventData data)
    {
        hoverBool = false;
        TooltipManager.Instance.DeactivateTooltip(inventoryButton, _newTooltip, _newTooltipBckg, inventoryTooltip);
    }

    void OnPointerOver()
    {
        TooltipManager.Instance.OnHoverButtonActivateTooltip(inventoryButton, _newTooltip, _newTooltipBckg, inventoryTooltip);
    }
}

