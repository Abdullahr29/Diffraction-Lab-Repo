using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoveTool : Tool
{
     void Start()
    {
        gameObject.GetComponent<Image>().sprite = TooltrayController.Instance._moveSprite;
    }

    GameObject moveController, confirmObject, denyObject;
    MoveFunction moveFunction;
    bool isBeingUsed = false;
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
        }        
    }

    public override void DeactivateButton()
    {
        moveFunction.AbruptEnd();
        moveController.SetActive(false);
        isBeingUsed = false;
    }
}
