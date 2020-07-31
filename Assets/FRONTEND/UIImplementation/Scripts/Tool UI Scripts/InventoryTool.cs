using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryTool : Tool
{
       public override void ButtonInteract()
    {
        ModalManager.Instance.ActivateInventory();
    }
}
