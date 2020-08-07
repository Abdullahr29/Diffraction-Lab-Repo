using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExportTool : Tool
{
    /*void Start()
    {
        gameObject.GetComponent<Image>().sprite = TooltrayController.Instance._exportprite;
    }*/

    bool isBeingUsed = false;

    public override void ButtonInteract()
    {
        isBeingUsed = !isBeingUsed;

        if (isBeingUsed)
        {
            TooltrayController.Instance.newTool = this;
            TooltrayController.Instance.SwitchTool();
            TooltrayController.Instance.activeTools.Add(this);
            UIController.Instance.mainCam.GetComponentInParent<CameraManager>().enabled = false;
        }
        else
        {
            UIController.Instance.mainCam.GetComponentInParent<CameraManager>().enabled = true;
        }
    }
}