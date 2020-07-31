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
    public override void ButtonInteract()
    {
        isBeingUsed = !isBeingUsed;
        if (moveController == null)
        {
            moveController = new GameObject("moveController");
            moveFunction = moveController.AddComponent<MoveFunction>();
        }        
        moveController.SetActive(isBeingUsed); //if button is active then enable MoveFunction to listen for input
        Debug.Log(moveFunction.enabled);
    }
 }
