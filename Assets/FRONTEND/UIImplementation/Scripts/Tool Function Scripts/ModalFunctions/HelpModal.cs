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

    void Start()
    { 
        CloseListeners(_helpModal);
    }
}