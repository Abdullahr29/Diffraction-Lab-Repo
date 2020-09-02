using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RotateTool : Tool
{
    private SpriteRenderer spriteRenderer;
    GameObject rotateController, confirmObject, denyObject;
    RotateFunction rotateFunction;
    bool isBeingUsed = false;

    GameObject _activeBckg;
    string activeName = "Object Rotate Active";

    GameObject _newTooltip, _newTooltipBckg;
    bool hoverBool;
    Button rotateButton;
    string rotateTooltip = "Object Rotate Tool";

    void OnEnable()
    {
        this.GetComponent<Image>().sprite = TooltrayController.Instance._rotateSprite;
        rotateButton = this.GetComponent<Button>();
    }

    void Update()
    {
        if (hoverBool) this.OnPointerOver();
    }

    public override void ButtonInteract()
    {
        isBeingUsed = !isBeingUsed;

        if (rotateController == null)
        {
            Debug.Log("ROTATE CONTROLLER NULL");
            rotateController = new GameObject("rotateController");
            rotateFunction = rotateController.AddComponent<RotateFunction>();            
        }
        rotateController.SetActive(isBeingUsed); //if button is active then enable rotateFunction to listen for input
        Debug.Log(isBeingUsed);
        ObjectManager.Instance.RotationController = rotateController;

        if (isBeingUsed)
        {
            Debug.Log("IS BEING USED");
            TooltrayController.Instance.newTool = this;
            TooltrayController.Instance.SwitchTool();
            TooltrayController.Instance.activeTools.Add(this);
            TooltrayController.Instance.ActiveToolBckg(_activeBckg, 2, true, activeName);
        }
        else
        {
            Debug.Log("IS NOT BEING USED");
            //Destroy(rotateController);
            TooltrayController.Instance.ActiveToolBckg(_activeBckg, 2, false, activeName);
            DeactivateButton();
        }
    }

    public override void DeactivateButton()
    {
        if (rotateController != null)
        {
            rotateFunction.AbruptEnd();
            rotateController.SetActive(false);
            TooltrayController.Instance.ActiveToolBckg(_activeBckg, 2, false, activeName);
            isBeingUsed = false;
        }        
    }


    public override void OnPointerEnter(PointerEventData data)
    {
        hoverBool = true;
    }

    public override void OnPointerExit(PointerEventData data)
    {
        hoverBool = false;
        TooltipManager.Instance.DeactivateTooltip(rotateButton, _newTooltip, _newTooltipBckg, rotateTooltip);
    }

    void OnPointerOver()
    {
        TooltipManager.Instance.OnHoverButtonActivateTooltip(rotateButton, _newTooltip, _newTooltipBckg, rotateTooltip);
    }

}

