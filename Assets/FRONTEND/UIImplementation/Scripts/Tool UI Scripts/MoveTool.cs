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
    public override void ButtonInteract()
    {
        isBeingUsed = !isBeingUsed;
        this.GetComponent<Image>().sprite = TooltrayController.Instance._moveSpriteActive;

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
            SetActiveSprite(TooltrayController.Instance.newTool, true);
        }
        SetActiveSprite(TooltrayController.Instance.newTool, false);      
    }

    public override void DeactivateButton()
    {
        if (moveController != null)
        {
            moveFunction.AbruptEnd();
            moveController.SetActive(false);
            isBeingUsed = false;
        }
    }

    public void SetActiveSprite(Tool tool, bool active)
    {
        if (active == true)
        {
            Debug.Log("changeSprite");
            tool.GetComponent<Image>().sprite = TooltrayController.Instance._moveSpriteActive;
        }
        tool.GetComponent<Image>().sprite = TooltrayController.Instance._moveSprite;
    }
}
