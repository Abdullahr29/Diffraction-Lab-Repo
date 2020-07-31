using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LabScriptTool : Tool
{
    public override void ButtonInteract()
    {
        ModalManager.Instance.ActivateLabScript();
    }
}
