using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Buttons
{
    public Button button;
    public string tooltip;

    public Buttons(Button button, string tooltip)
    {
        this.button = button;
        this.tooltip = tooltip;
    }
}

public class TooltipManager : MonoBehaviour
{
    // Make manager into singleton
    private static TooltipManager _instance;
    public static TooltipManager Instance
    {   get
    {
            if (_instance == null)
                Debug.LogError("TooltipManager is NULL.");

            return _instance;

        }
    }
    
    private void Awake()
    {
        _instance = this; 
    }

    public Dictionary<Button, string> buttonsDictTooltips = new Dictionary<Button, string>();

    public Transform tooltipsParent;


    public void addNewButton(Button thisButton, string buttonTooltip)
    {
        buttonsDictTooltips.Add(thisButton, buttonTooltip);
        Debug.Log(buttonsDictTooltips);
    }

    private string returnTooltipFromKey(Button button)
    {
        return buttonsDictTooltips[button];
    }

    public void ActivateTooltip(Button _hoveredButton, GameObject _newTooltip, GameObject _newTooltipBckg, bool active)
    {
        string tooltipText = returnTooltipFromKey(_hoveredButton);
        string tooltipTextNoSpaces = tooltipText.Replace(" ", string.Empty);

        Transform tooltipChild = tooltipsParent.Find(tooltipTextNoSpaces);
        if (active == false)
        {
            if(tooltipChild == null)
            {
                // Create newTooltip background in tooltipsParent object in UI layer
                _newTooltipBckg = new GameObject();
                _newTooltipBckg.transform.SetParent(tooltipsParent, false);
                _newTooltipBckg.name = tooltipTextNoSpaces;
                RectTransform tooltipBckgRT = _newTooltipBckg.AddComponent<RectTransform>();
                _newTooltipBckg.AddComponent<Image>().color = new Color(243, 242, 230, 255);

                // Create text object with tooltip name in tooltip background, add TMPUGI to it
                _newTooltip = new GameObject();
                _newTooltip.transform.SetParent(_newTooltipBckg.transform, false);
                _newTooltip.name = tooltipTextNoSpaces + "Text";
                TextMeshProUGUI tooltipTMP = _newTooltip.AddComponent<TextMeshProUGUI>();
                tooltipTMP.SetText(tooltipText);
                tooltipTMP.color = new Color(17, 17, 17, 255);

                // Calculate the size of the box depending on the text length
                tooltipBckgRT.sizeDelta = new Vector2(5, 5) + _newTooltip.GetComponent<RectTransform>().sizeDelta;

            } else {
                tooltipChild.gameObject.SetActive(false);
            }
        }
        else {
            tooltipChild.gameObject.SetActive(true);
        }

    }

    public void onHoverButtonCreateTooltip(Button _hoveredButton, GameObject _newTooltip, GameObject _newTooltipBckg, bool hoverBool)
    {
        Vector2 mp = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        hoverBool = isMouseOverButton(_hoveredButton);

        if (hoverBool == true)
        {
            Debug.Log("Tooltip");
            ActivateTooltip(_hoveredButton, _newTooltip, _newTooltipBckg, true);
        }
        else {
            ActivateTooltip(_hoveredButton, _newTooltip, _newTooltipBckg, false);
        }
    }


    public bool isMouseOverButton(Button _hoveredButton)
    {
        RectTransform buttonRectTransform = _hoveredButton.gameObject.GetComponent<RectTransform>();

        Vector2 localMousePosition = buttonRectTransform.InverseTransformPoint(Input.mousePosition);
        if (buttonRectTransform.rect.Contains(localMousePosition))
        {
            return true;
        }
        else {
            return false;
        }
    }

}
