using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoveTool : Tool
{

    GameObject moveController, confirmObject, denyObject;
    MoveFunction moveFunction;
    bool isBeingUsed = false;

    GameObject _activeBckg;
    string activeName = "Object Move Active";

    GameObject _newTooltip, _newTooltipBckg;
    bool hoverBool;
    Button moveButton;
    string moveTooltip = "Object Move Tool";
    void OnEnable()
    {
        this.GetComponent<Image>().sprite = TooltrayController.Instance._moveSprite;
        moveButton = this.GetComponent<Button>();
    }


    public override void ButtonInteract()
    {
        isBeingUsed = !isBeingUsed;

        if (moveController == null)
        {
            moveController = new GameObject("moveController");
            moveFunction = moveController.AddComponent<MoveFunction>();
        }        
        moveController.SetActive(isBeingUsed); //if button is active then enable MoveFunction to listen for input
        Debug.Log(isBeingUsed);

        if (isBeingUsed)
        {
            TooltrayController.Instance.newTool = this;
            TooltrayController.Instance.SwitchTool();
            TooltrayController.Instance.activeTools.Add(this);
            TooltrayController.Instance.ActiveToolBckg(_activeBckg, 1, true, activeName);
        }
        else
        {
            TooltrayController.Instance.ActiveToolBckg(_activeBckg, 1, false, activeName);
        }
    }

    public override void DeactivateButton()
    {
        if (moveController != null)
        {
            moveFunction.AbruptEnd();
            moveController.SetActive(false);
            TooltrayController.Instance.ActiveToolBckg(_activeBckg, 1, false, activeName);
            isBeingUsed = false;
        }
    }

    public override void OnPointerEnter(PointerEventData data)
    {
        TooltipManager.Instance.OnHoverButtonActivateTooltip(moveButton, _newTooltip, _newTooltipBckg, moveTooltip);
    }

    public override void OnPointerExit(PointerEventData data)
    {
        TooltipManager.Instance.DeactivateTooltip(moveButton, _newTooltip, _newTooltipBckg, moveTooltip);
    }
}
