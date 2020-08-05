using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TakeDataTool : Tool
{
    public DetectorMeasurementControl_LOCKED detectorMeasure;

   
    void OnEnable()
    {
        gameObject.GetComponent<Image>().sprite = TooltrayController.Instance._takeDataSprite;
        detectorMeasure = GameObject.Find("v2 DETECTOR").GetComponentInChildren<DetectorMeasurementControl_LOCKED>();
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
            //cam.transform.position = new Vector3(-499.9f, 488.04f, 49.6f);                
        }

        UIController.Instance.SwitchCams(isBeingUsed);

        UIController.Instance.emailManager.SetActive(isBeingUsed);
        detectorMeasure.OnChange(isBeingUsed);
    }

    public override void DeactivateButton()
    {
        GameObject.Find("CameraFocus").transform.position = new Vector3(0, 10f, -40f);
        GameObject.Find("CameraFocus").GetComponent<CameraManager>().enabled = true;
        UIController.Instance.emailManager.SetActive(false);
        detectorMeasure.OnChange(false);
        UIController.Instance.SwitchCams(false);      
    }

}
