using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TooltipManager : MonoBehaviour
{
    // Make manager into singleton
    private static TooltipManager _instance;
    Vector2Int screen;
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

    public Transform tooltipsParent;
    public Sprite tooltipSprite;




    public void OnHoverButtonActivateTooltip(Button _hoveredButton, GameObject _newTooltip, GameObject _newTooltipBckg, string buttonTooltip)
    {
        string tooltipText = buttonTooltip;
        string tooltipTextNoSpaces = tooltipText.Replace(" ", string.Empty);

        Transform _tooltipChild = tooltipsParent.Find(tooltipTextNoSpaces);

        Vector2 mp = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        if (_tooltipChild != null) {
            _tooltipChild.gameObject.SetActive(true);
        }
        else
        {
            SetUpTooltip(_hoveredButton, _newTooltip, _newTooltipBckg, tooltipText, tooltipTextNoSpaces);
        }
    }

    public void DeactivateTooltip(Button _hoveredButton, GameObject _newTooltip, GameObject _newTooltipBckg, string buttonTooltip)
    {
        string tooltipTextNoSpaces = buttonTooltip.Replace(" ", string.Empty);
        
        Transform tooltipExists = tooltipsParent.Find(tooltipTextNoSpaces);
        if (tooltipExists != null)
        {
            tooltipExists.gameObject.SetActive(false);
        }
    }




    private void SetUpTooltip(Button _hoveredButton, GameObject _newTooltip, GameObject _newTooltipBckg, string tooltipText, string tooltipTextNoSpaces)
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
        RectTransform tooltipRT = _newTooltip.AddComponent<RectTransform>();
        _newTooltip.name = tooltipTextNoSpaces + "Text";

        TextMeshProUGUI tooltipTMP = _newTooltip.AddComponent<TextMeshProUGUI>();
        tooltipTMP.text = tooltipText;
        tooltipTMP.color = new Color32(17, 17, 17, 255);
        tooltipTMP.font = Resources.Load("RalewayRegular SDF", typeof(TMP_FontAsset)) as TMP_FontAsset;
        tooltipTMP.fontSize = 16f;
        tooltipTMP.autoSizeTextContainer = true;
        Canvas.ForceUpdateCanvases();


        // Calculate the size of the box depending on the text length
        tooltipBckgRT.sizeDelta = new Vector2(15, 15) + tooltipRT.sizeDelta;
        float mouseX = Input.mousePosition.x;
        float mouseY = Input.mousePosition.y;

        tooltipBckgRT.position = new Vector3(mouseX + 50, mouseY -50, 0);
        tooltipBckgRT.gameObject.GetComponent<Image>().sprite = tooltipSprite;
    }

}
