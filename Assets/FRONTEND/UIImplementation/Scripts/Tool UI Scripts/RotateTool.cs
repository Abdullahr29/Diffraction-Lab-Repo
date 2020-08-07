using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RotateTool : Tool
{
    private SpriteRenderer spriteRenderer;
    void OnEnable()
    {
        gameObject.GetComponent<Image>().sprite = TooltrayController.Instance._rotateSprite;
    }

    GameObject rotateController, confirmObject, denyObject;
    RotateFunction rotateFunction;
    GameObject _activeBckg;
    bool isBeingUsed = false;

    public override void ButtonInteract()
    {
        isBeingUsed = !isBeingUsed;

        if (rotateController == null)
        {
            Debug.Log("ROTATE CONTROLLER NULL");
            rotateController = new GameObject("rotateController");
            rotateFunction = rotateController.AddComponent<RotateFunction>();
        }
        rotateController.SetActive(isBeingUsed); //if button is active then enable MoveFunction to listen for input
        Debug.Log(isBeingUsed);

        if (isBeingUsed)
        {
            Debug.Log("IS BEING USED");
            TooltrayController.Instance.newTool = this;
            TooltrayController.Instance.SwitchTool();
            TooltrayController.Instance.activeTools.Add(this);
            TooltrayController.Instance.ActiveToolBckg(_activeBckg, 2, true, UIController.Instance.currentMode);
        }
        else
        {
            Debug.Log("IS NOT BEING USED");
            Destroy(rotateController);
            TooltrayController.Instance.ActiveToolBckg(_activeBckg, 2, false, UIController.Instance.currentMode);
            DeactivateButton();
        }
    }

    public override void DeactivateButton()
    {
        if (rotateController != null)
        {
            rotateFunction.AbruptEnd();
            rotateController.SetActive(false);
            TooltrayController.Instance.ActiveToolBckg(_activeBckg, 2, false, UIController.Instance.currentMode);
            isBeingUsed = false;
        }        
    }

}

