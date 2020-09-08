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
    Button exportButton;

    void OnEnable()
    {
        this.GetComponent<Image>().sprite = TooltrayController.Instance._exportSprite;
        exportButton = this.GetComponent<Button>();
    }

    public override void ButtonInteract()
    {
        isBeingUsed = !isBeingUsed;

        if (isBeingUsed)
        {
            TooltrayController.Instance.newTool = this;
            TooltrayController.Instance.SwitchTool();
            TooltrayController.Instance.activeTools.Add(this);            
        }
        else
        {
            
        }

        ObjectManager.Instance.EmailManager.transform.Find("Canvas").gameObject.SetActive(isBeingUsed);
    }
}