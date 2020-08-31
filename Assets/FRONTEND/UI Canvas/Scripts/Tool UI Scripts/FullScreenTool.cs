using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FullScreenTool : Tool
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
        Debug.Log("FullScreen - should work in app, need to test");

        // Need to test in actual app
        Screen.fullScreen = !Screen.fullScreen;
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
