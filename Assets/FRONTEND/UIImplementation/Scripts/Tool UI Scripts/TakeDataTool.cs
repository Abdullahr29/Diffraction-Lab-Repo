using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TakeDataTool : Tool
{
     void Start()
    {
        gameObject.GetComponent<Image>().sprite = TooltrayController.Instance._takeDataSprite;
    }

    bool isBeingUsed = false;

    public override void ButtonInteract()
    {
        isBeingUsed = !isBeingUsed;

        if (isBeingUsed)
        {
            TooltrayController.Instance.newTool = this;
            TooltrayController.Instance.SwitchTool();
            TooltrayController.Instance.activeTools.Add(this);
        }
    }
}
