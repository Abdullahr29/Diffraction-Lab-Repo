using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RotateTool : Tool
{
    GameObject rotateController, confirmObject, denyObject;
    RotateFunction rotateFunction;
    bool isBeingUsed = false;
    public override void ButtonInteract()
    {
        isBeingUsed = !isBeingUsed;
        if (rotateController == null)
        {
            rotateController = new GameObject("rotateController");
            rotateFunction = rotateController.AddComponent<RotateFunction>();
        }
        rotateController.SetActive(isBeingUsed); //if button is active then enable MoveFunction to listen for input
        Debug.Log(rotateFunction.enabled);
    }
}
