using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IntroductorySpielModal : Modal
{
    void Awake ()
    {
        // Define all of the variables required, inherited from parent Modal class
        _modal = this.gameObject;
        _modalOverlay = this.transform.GetChild(0).GetComponentInParent<Button>();
        _closeModalBtn = this.transform.GetChild(1).GetChild(3).GetComponentInParent<Button>();
        _nextBtn = this.transform.GetChild(1).GetChild(4).GetComponentInParent<Button>();
    }

    void Start()
    {
        // Deactivate camera input control listeners and activate close modal listeners
        DeactivateInputManager();
        TooltrayController.Instance.dynamicTray.SetActive(false);
        CloseSpielListeners();
    }
}