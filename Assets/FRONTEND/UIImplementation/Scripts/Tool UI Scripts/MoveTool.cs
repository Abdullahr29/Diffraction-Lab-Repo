using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoveTool : Tool
{
    void OnEnable()
    {
        this.GetComponent<Image>().sprite = TooltrayController.Instance._moveSprite;
    }

    GameObject moveController, confirmObject, denyObject;
    MoveFunction moveFunction;
    bool isBeingUsed = false;
    GameObject _activeBckg;
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
            TooltrayController.Instance.ActiveToolBckg(_activeBckg, 1, true, UIController.Instance.currentMode);
        }
        else
        {
            TooltrayController.Instance.ActiveToolBckg(_activeBckg, 1, false, UIController.Instance.currentMode);
        }
    }

    public override void DeactivateButton()
    {
        if (moveController != null)
        {
            moveFunction.AbruptEnd();
            moveController.SetActive(false);
            TooltrayController.Instance.ActiveToolBckg(_activeBckg, 1, false, UIController.Instance.currentMode);
            isBeingUsed = false;
        }
    }
}
