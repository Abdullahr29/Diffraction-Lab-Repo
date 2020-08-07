using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryTool : Tool
{
    private GameObject _activeBckg;
    GameObject _newTooltip;
    GameObject _newTooltipBckg;
    bool hoverBool;
    Button inventoryButton;

    void OnEnable()
    {
        gameObject.GetComponent<Image>().sprite = TooltrayController.Instance._inventorySprite;
    }
        /*
        inventoryButton = this.GetComponent<Button>();
        TooltipManager.Instance.addNewButton(inventoryButton, "Inventory");
    }

    void Update()
    {
        TooltipManager.Instance.onHoverButtonCreateTooltip(inventoryButton, _newTooltip, _newTooltipBckg, hoverBool);
    }*/

    bool isBeingUsed = false;
    public override void ButtonInteract()
    {
        isBeingUsed = !isBeingUsed;
        if (isBeingUsed)
        {
            TooltrayController.Instance.ActiveToolBckg(_activeBckg, 0, true, UIController.Instance.currentMode);
            ModalManager.Instance.ActivateInventory();
        }

        if (isBeingUsed)
        {
            TooltrayController.Instance.newTool = this;
            TooltrayController.Instance.SwitchTool();
            TooltrayController.Instance.activeTools.Add(this);
            UIController.Instance.inputManager.SetActive(false);
        }
    }

    public override void DeactivateButton()
    {

    }
}

