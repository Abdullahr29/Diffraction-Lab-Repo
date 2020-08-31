using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Modal : MonoBehaviour
{   
    // Define all variables to be inherited by Modal children
    protected GameObject _modal;
    protected GameObject _activeBckg;
    protected Button _modalOverlay;
    protected Button _closeModalBtn;
    protected Button _previousBtn;
    protected Button _nextBtn;

    public virtual void CloseModal()
    {
        // Deactivate modal and active tool cue
        _modal.SetActive (false);
        _activeBckg.SetActive(false);
        UIController.Instance.inputManager.SetActive(true);
    }

    public virtual void CloseListeners()
    {
        // Add listeners and close modal
        _modalOverlay.onClick.AddListener(delegate{CloseModal(); });
        _closeModalBtn.onClick.AddListener(delegate{CloseModal();});
    }

    public virtual void CloseSpielListeners()
    {
        // Same as above but for introductory spiel - only happens at the start, closing it
        // activates the camera manager for the first time.

        _closeModalBtn.onClick.AddListener(delegate {
            _modal.SetActive(false);
            TooltrayController.Instance.dynamicTray.SetActive(true);
            UIController.Instance.calibrateClick();
            UIController.Instance.mainCam.GetComponentInParent<CameraManager>().enabled = true;
        });

        _nextBtn.onClick.AddListener(delegate {
            ModalManager.Instance.ChangeStep(true, _modal);
        });

        _modalOverlay.onClick.AddListener(delegate { _nextBtn.onClick.Invoke(); });
    }

    public virtual void CloseTutorialListeners()
    {
        // Same as above but for introductory spiel - only happens at the start, closing it
        // activates the camera manager for the first time.
    
        _closeModalBtn.onClick.AddListener(delegate{
            _modal.SetActive (false);
            TooltrayController.Instance.dynamicTray.SetActive(true);
            UIController.Instance.calibrateClick();
            UIController.Instance.mainCam.GetComponentInParent<CameraManager>().enabled = true;
        });

        _previousBtn.onClick.AddListener(delegate { ModalManager.Instance.ChangeStep(false, _modal); });

        if(_modal.name == "TakeDataToolsTutorial(Clone)")
        {
            _modalOverlay.onClick.AddListener(delegate { _closeModalBtn.onClick.Invoke(); });
        } else
        {
            _nextBtn.onClick.AddListener(delegate {
                ModalManager.Instance.ChangeStep(true, _modal);
            });

            _modalOverlay.onClick.AddListener(delegate { _nextBtn.onClick.Invoke(); });
        }
    }

    public virtual void DeactivateInputManager()
    {
        UIController.Instance.isActiveInputManager(false);
    }
}