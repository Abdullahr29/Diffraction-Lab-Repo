using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuitTool : Tool
{
    [SerializeField]
    Sprite _originalSprite;
    [SerializeField]
    Sprite _hoverSprite;
    Image quitButton;

    void OnEnable()
    {
        quitButton = this.GetComponent<Image>();
    }

    public override void ButtonInteract()
    {
        Debug.Log("Quit - only works in app");
        Application.Quit();
    }

    public override void OnPointerEnter(PointerEventData data)
    {
        quitButton.sprite = _hoverSprite;
    }

    public override void OnPointerExit(PointerEventData data)
    {
        quitButton.sprite = _originalSprite;
    }



}
