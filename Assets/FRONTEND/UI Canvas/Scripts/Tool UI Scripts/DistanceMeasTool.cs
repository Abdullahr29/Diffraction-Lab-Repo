﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DistanceMeasTool : Tool
{
    GameObject measurementController;
    MeasurementControl measurementControl;
    bool isBeingUsed = false;

    GameObject _activeBckg;
    string activeName = "Length Measurement Active";

    GameObject _newTooltip, _newTooltipBckg;
    bool hoverBool;
    Button measureButton;
    string measureTooltip = "Length Measurement Tool";

    void OnEnable()
    {
        gameObject.GetComponent<Image>().sprite = TooltrayController.Instance._measureSprite;
        measureButton = this.GetComponent<Button>();
        measurementController = ObjectManager.Instance.MeasureController;
    }

    void Update()
    {
        if (hoverBool) this.OnPointerOver();
    }

    public override void ButtonInteract()
    {
        //Call initialisation method for the relevant tool
        isBeingUsed = !isBeingUsed;        
        if (measurementController == null)
        {
            measurementControl = measurementController.AddComponent<MeasurementControl>();            
        }
        else
        {
            measurementControl = measurementController.GetComponent<MeasurementControl>();
        }
        measurementControl.OnChange(isBeingUsed); //if button is active then enable MoveFunction to listen for input
        Debug.Log(isBeingUsed);

        ObjectManager.Instance.MeasureController = measurementController;


        if (isBeingUsed)
        {
            TooltrayController.Instance.newTool = this;
            TooltrayController.Instance.SwitchTool();
            TooltrayController.Instance.activeTools.Add(this);
            TooltrayController.Instance.ActiveToolBckg(_activeBckg, 0, true, activeName);
        }
        else
        {
            TooltrayController.Instance.ActiveToolBckg(_activeBckg, 0, false, activeName);
        }
    }

    public override void DeactivateButton()
    {
        if (measurementControl != null)
        {
            measurementControl.AbruptEnd();
            measurementControl.OnChange(false);
        }
        
        TooltrayController.Instance.ActiveToolBckg(_activeBckg, 1, false, activeName);
        isBeingUsed = false;
        //TooltrayController.Instance.ActiveToolBckg(_activeBckg, 0, false, activeName);
    }

    public override void OnPointerEnter(PointerEventData data)
    {
        hoverBool = true;
    }

    public override void OnPointerExit(PointerEventData data)
    {
        hoverBool = false;
        TooltipManager.Instance.DeactivateTooltip(measureButton, _newTooltip, _newTooltipBckg, measureTooltip);
    }

    void OnPointerOver()
    {
        TooltipManager.Instance.OnHoverButtonActivateTooltip(measureButton, _newTooltip, _newTooltipBckg, measureTooltip);
    }
}