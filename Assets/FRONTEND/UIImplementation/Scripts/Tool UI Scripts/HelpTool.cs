using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HelpTool : Tool
{
    public override void ButtonInteract()
    {
        ModalManager.Instance.ActivateHelpModal(UIController.Instance.currentMode);
    }
}