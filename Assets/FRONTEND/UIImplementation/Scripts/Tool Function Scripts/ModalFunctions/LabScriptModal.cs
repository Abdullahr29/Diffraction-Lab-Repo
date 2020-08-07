using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LabScriptModal : Modal
{   
    //public Mode currentMode;
    [SerializeField]
    private GameObject _labScriptModal;
    GameObject _activeBckg;

    void Start()
    { 
        UIController.Instance.inputManager.SetActive(false);
        _activeBckg = TooltrayController.Instance.mainTooltray.transform.GetChild(0).GetChild(0).gameObject;
        CloseListeners(_labScriptModal, _activeBckg);
    }
}