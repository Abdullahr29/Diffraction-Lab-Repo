using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IntroductorySpielModal : Modal
{   
    //public Mode currentMode;
    [SerializeField]
    private GameObject _labScriptModal;

    void Start()
    { 
        CloseListeners(_labScriptModal);
    }
}