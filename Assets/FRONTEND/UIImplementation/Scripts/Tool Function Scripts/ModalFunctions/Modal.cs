using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Modal : MonoBehaviour
{   
    public Button _modalOverlay, _closeModalBtn;

    public virtual void CloseModal(GameObject _modal)
    {
        _modal.SetActive (false);
    }

    public virtual void CloseListeners(GameObject _modal)
    {
        _modalOverlay.onClick.AddListener(delegate{CloseModal(_modal);});
        _closeModalBtn.onClick.AddListener(delegate{CloseModal(_modal);});
    }
}