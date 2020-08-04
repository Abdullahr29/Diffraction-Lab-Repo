using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryTool : Tool
{
     void OnEnable()
    {
        gameObject.GetComponent<Image>().sprite = TooltrayController.Instance._inventorySprite;
    }

    bool isBeingUsed = false;
    public override void ButtonInteract()
    {
        isBeingUsed = !isBeingUsed;
        if (isBeingUsed)
        {
            ModalManager.Instance.ActivateInventory();
        }

        if (isBeingUsed)
        {
            TooltrayController.Instance.newTool = this;
            TooltrayController.Instance.SwitchTool();
            TooltrayController.Instance.activeTools.Add(this);
        }
    }

     public override void DeactivateButton()
    {
        
    }
}

