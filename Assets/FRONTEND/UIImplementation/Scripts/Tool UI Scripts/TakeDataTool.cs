using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TakeDataTool : Tool
{
     void OnEnable()
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
            GameObject.Find("CameraFocus").transform.position = new Vector3(-499.9f, 499f, 9.5f);
            UIController.Instance.emailManager.SetActive(true);
        }
    }

    public override void DeactivateButton()
    {
        GameObject.Find("CameraFocus").transform.position = new Vector3(0, 10f, -40f);
        UIController.Instance.emailManager.SetActive(false);
    }

}
