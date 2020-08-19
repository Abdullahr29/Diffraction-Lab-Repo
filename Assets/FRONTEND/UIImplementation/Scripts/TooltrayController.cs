using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltrayController : MonoBehaviour
{
    public GameObject buttonPrefab;
    public List<GameObject> dynamicButtons;
    public GameObject dynamicTray;
    public Transform tempButtonsObject;
    public Transform mainTooltray;
    RectTransform tooltrayRect;
    public Tool newTool;


    [Header("Dynamic Tool Sprites")]
    public Sprite _inventorySprite;
    public Sprite _moveSprite, _rotateSprite, _measureSprite, _angleSprite, _investigateSprite, _takeDataSprite;
    public Sprite _inventorySpriteActive, _moveSpriteActive, _rotateSpriteActive, _measureSpriteActive, _angleSpriteActive, _investigateSpriteActive, _takeDataSpriteActive, _labScriptSpriteActive, _helpSpriteActive;

    [Header("Dynamic Tool Sprites")]
    public Sprite _confirmSprite;
    public Sprite _denySprite;

    float minheight = 215f;
    float buttonUnitheight = 76f;

    int extraButtons;
    int maxDynamicButtons = 3;

    List<Tool> toolsInMode;
    public List<Tool> activeTools;


    private static TooltrayController _instance;
    public static TooltrayController Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("TooltrayController is NULL.");

            return _instance;

        }
    }

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        //Creates the blank buttons before we apply tools to them, held in dynamicTools


        toolsInMode = new List<Tool>();
        activeTools = new List<Tool>();
        dynamicButtons = new List<GameObject>();
        for (int i = 0; i < maxDynamicButtons; i++)
        {
            GameObject tempButton = Instantiate(buttonPrefab);
            tempButton.transform.SetParent(dynamicTray.transform); //Places buttons in parents vertical layout
            tempButton.transform.localScale = new Vector3(1, 1, 1);
            dynamicButtons.Add(tempButton);
        }
        
        tooltrayRect = gameObject.GetComponent<RectTransform>();            
    }

    public void SetTrayContents(Mode desiredMode)
    {
        if (toolsInMode != null)
        {
            foreach (var tool in toolsInMode) //tool here refers to the specific Tool script
            {                
                tool.DeactivateButton();
                tool.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                tool.enabled = false;
            }
            toolsInMode.Clear();
        }        

        switch (desiredMode)
        {     
            //Depending on the current mode different tools are created/enabled

            case Mode.Calibrate:                
                extraButtons = 3;

                CreateTool<InventoryTool>(dynamicButtons[0]);
                CreateTool<MoveTool>(dynamicButtons[1]);
                CreateTool<RotateTool>(dynamicButtons[2]);                
                break;

            case Mode.Measure:
                extraButtons = 2;

                CreateTool<DistanceMeasTool>(dynamicButtons[0]);
                CreateTool<AngleMeasTool>(dynamicButtons[1]);
                dynamicButtons[2].SetActive(false); 
                break;

            case Mode.Explore:
                extraButtons = 3;

                CreateTool<InventoryTool>(dynamicButtons[0]);
                CreateTool<MoveTool>(dynamicButtons[1]);
                CreateTool<InvestigateTool>(dynamicButtons[2]);
                break;

            case Mode.DataTake:
                extraButtons = 1;

                CreateTool<TakeDataTool>(dynamicButtons[0]);
                dynamicButtons[1].SetActive(false);
                dynamicButtons[2].SetActive(false);
                break;
        }
        tooltrayRect.sizeDelta = new Vector2(tooltrayRect.sizeDelta.x, minheight + (extraButtons * buttonUnitheight));
    }

    void CreateTool<T>(GameObject rootObject) where T : Tool
    {
        //Generic method to create Tools from their classes that inherit from Tool

        T tool = rootObject.GetComponent<T>();
        if (tool == null) //check to see if we already have a instance of this Tool attached
        {
            rootObject.AddComponent<T>();
            tool = rootObject.GetComponent<T>();
        }
        tool.enabled = true;
        rootObject.SetActive(true);
        toolsInMode.Add(tool);
        rootObject.GetComponent<Button>().onClick.AddListener(tool.ButtonInteract);
    }

    public void SwitchTool()
    {        
        if (activeTools != null)
        {
            foreach (var tool in activeTools)
            {
                if (tool != newTool)
                {
                    tool.DeactivateButton();
                }
            }
            activeTools.Clear();
        }                          
    }

    private void SetUpDynamicActiveTool(GameObject _activeBckg, int n, string nameActive)
    {
        _activeBckg = new GameObject();
        _activeBckg.transform.SetParent(dynamicTray.transform.GetChild(n), false);

        _activeBckg.AddComponent<RectTransform>().sizeDelta = new Vector2(55, 55);
        _activeBckg.AddComponent<Image>();   

        switch (UIController.Instance.currentMode)
        {    

            case Mode.Calibrate: 
                if (n == 0)
                {
                    Debug.Log("inventorySprite");
                    _activeBckg.GetComponent<Image>().sprite = TooltrayController.Instance._inventorySpriteActive;
                    _activeBckg.name = nameActive;
                }
                else if (n == 1)
                {
                    _activeBckg.GetComponent<Image>().sprite = TooltrayController.Instance._moveSpriteActive;
                    _activeBckg.name = nameActive;
                }
                else if (n == 2)
                {
                    _activeBckg.GetComponent<Image>().sprite = TooltrayController.Instance._rotateSpriteActive;
                    _activeBckg.name = nameActive;
                }
                break;

            case Mode.Measure:
                if (n == 0)
                {
                    _activeBckg.GetComponent<Image>().sprite = TooltrayController.Instance._measureSpriteActive;
                    _activeBckg.name = nameActive;
                }
                else if (n == 1)
                {
                    _activeBckg.GetComponent<Image>().sprite = TooltrayController.Instance._angleSpriteActive;
                    _activeBckg.name = nameActive;
                }
                break;

            case Mode.Explore:
                if (n == 0)
                {
                    _activeBckg.GetComponent<Image>().sprite = TooltrayController.Instance._inventorySpriteActive;
                    _activeBckg.name = nameActive;
                }
                else if (n == 1)
                {
                    _activeBckg.GetComponent<Image>().sprite = TooltrayController.Instance._moveSpriteActive;
                    _activeBckg.name = nameActive;
                }
                else if (n == 2)
                {
                    _activeBckg.GetComponent<Image>().sprite = TooltrayController.Instance._investigateSpriteActive;
                    _activeBckg.name = nameActive;
                }
                break;

            case Mode.DataTake: 
                if (n == 0)
                {
                    _activeBckg.GetComponent<Image>().sprite = TooltrayController.Instance._takeDataSpriteActive;
                    _activeBckg.name = nameActive;
                }
                break;
        }
    }

    public void ActiveToolBckg(GameObject _activeBckg, int n, bool active, string nameActive)
    {
        Transform activeSprite = dynamicTray.transform.GetChild(n).Find(nameActive);

        if (active == true)
        {
            if (activeSprite != null)
            {
                activeSprite.gameObject.SetActive(true);
            }
            else if (activeSprite == null)
            {
                SetUpDynamicActiveTool(_activeBckg, n, nameActive);
            }
        }
        else if (active == false)
        {
            activeSprite.gameObject.SetActive(false);
        }
    }

    public void ActiveStaticToolBckg(GameObject _activeBckg, int n, bool active)
    {
        if (active == false)
        {
            mainTooltray.GetChild(n).GetChild(0).gameObject.SetActive(false);
        }

        if(mainTooltray.GetChild(n).childCount > 0)
        {
            mainTooltray.GetChild(n).GetChild(0).gameObject.SetActive(true);
        }
        else if(mainTooltray.GetChild(n).childCount == 0)
        {
            _activeBckg = new GameObject();
            _activeBckg.transform.SetParent(mainTooltray.GetChild(n), false);

            _activeBckg.AddComponent<RectTransform>().sizeDelta = new Vector2(55, 55);
            _activeBckg.AddComponent<Image>();  

            if (n == 0)
            {
                _activeBckg.GetComponent<Image>().sprite = TooltrayController.Instance._labScriptSpriteActive;
            }

            else if (n == 1)
            {
                _activeBckg.GetComponent<Image>().sprite = TooltrayController.Instance._helpSpriteActive;
            }
        }
    }
}
