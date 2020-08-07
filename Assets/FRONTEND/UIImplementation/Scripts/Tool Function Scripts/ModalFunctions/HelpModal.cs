using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HelpModal : Modal
{
    //public Mode currentMode;
    [SerializeField]
    private GameObject _helpModal;
    private GameObject _activeBckg;

    void Start()
    { 
        UIController.Instance.inputManager.SetActive(false);
        _activeBckg = TooltrayController.Instance.mainTooltray.transform.GetChild(1).GetChild(0).gameObject;
        CloseListeners(_helpModal, _activeBckg);
    }
}