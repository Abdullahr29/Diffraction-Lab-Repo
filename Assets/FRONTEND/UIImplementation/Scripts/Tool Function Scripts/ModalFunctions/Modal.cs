using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Modal : MonoBehaviour
{   
    public Button _modalOverlay, _closeModalBtn;

    public virtual void CloseModal(GameObject _modal, GameObject _activeBckg)
    {
        _modal.SetActive (false);
        _activeBckg.SetActive(false);
        UIController.Instance.inputManager.SetActive(true);
    }

    public virtual void CloseListeners(GameObject _modal, GameObject _activeBckg)
    {
        _modalOverlay.onClick.AddListener(delegate{CloseModal(_modal, _activeBckg);});
        _closeModalBtn.onClick.AddListener(delegate{CloseModal(_modal, _activeBckg);});
    }

    public virtual void CloseSpielListeners(GameObject _modal)
    {
        _modalOverlay.onClick.AddListener(delegate{
            _modal.SetActive (false);
            UIController.Instance.mainCam.GetComponentInParent<CameraManager>().enabled = true;
            });
        _closeModalBtn.onClick.AddListener(delegate{
            _modal.SetActive (false);
            UIController.Instance.inputManager.SetActive(true);
        });
    }
}