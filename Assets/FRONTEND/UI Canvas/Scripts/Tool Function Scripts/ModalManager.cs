using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ModalManager : MonoBehaviour
{
    // Turn modal manager into singleton
    private static ModalManager _instance;
    public static ModalManager Instance
    { get
        {
            if (_instance == null)
                Debug.LogError("ModalManager is NULL.");

            return _instance;

        }
    }

    private void Awake()
    {
        _instance = this;
    }

    [Header("Modal Prefabs")]
    // Find prefabs to instantiate
    [SerializeField] private GameObject _helpCalPrefab;
    [SerializeField] private GameObject _labScriptPrefab;
    [SerializeField] private GameObject _helpMesPrefab;
    [SerializeField] private GameObject _helpExplPrefab;
    [SerializeField] private GameObject _helpDataPrefab;
    [SerializeField] private GameObject _inventoryPrefab;
    [SerializeField] private GameObject _introSpielPrefab;
    [SerializeField] private GameObject _modesTutorial;
    [SerializeField] private GameObject _setUpTutorial;
    [SerializeField] private GameObject _measureTutorial;
    [SerializeField] private GameObject _exploreTutorial;
    [SerializeField] private GameObject _takeDataTutorial;


    // Empty game objects to instantiate the prefabs into
    public static GameObject helpCal, helpMes, helpExpl, helpData, labScript, inventory;
    public static GameObject introSpiel, modesTutorial, setUpTutorial, measureTutorial, exploreTutorial, takeDataTutorial;

    // These help check if prefabs already instantiated
    private List<string> createdModals = new List<string>();

    private Dictionary<string, GameObject> createdModalsDict = new Dictionary<string, GameObject>();

    public void ActivateModal(Dictionary<string, GameObject> exists, string name, GameObject prefab, GameObject modal, Transform parent)
    {
        // If modal not in hirearchy, instantiate prefab
        if (createdModalsDict.ContainsKey(name) == false)
        {
            modal = Instantiate(prefab, parent);
            modal.SetActive(true);
            exists.Add(name, modal);
        }
        
        exists[name].SetActive(true);

    }

    public void ActivateHelpModal(Mode desiredMode)
    {
        switch (desiredMode)
        {     
            //Depending on the current mode different modals are created/enabled

            case Mode.Calibrate:

                ActivateModal(createdModalsDict, "calibrateHelp", _helpCalPrefab, helpCal, this.transform.GetChild(1));
                break;

            case Mode.Measure:

                ActivateModal(createdModalsDict, "measureHelp", _helpMesPrefab, helpMes, this.transform.GetChild(1));
                break;

            case Mode.Explore:

                ActivateModal(createdModalsDict, "exploreHelp", _helpExplPrefab, helpExpl, this.transform.GetChild(1));
                break;

            case Mode.DataTake:

                ActivateModal(createdModalsDict, "dataHelp", _helpDataPrefab, helpData, this.transform.GetChild(1));
                break;
        }
    }

    public void ActivateLabScript()
    {
        ActivateModal(createdModalsDict, "labScript", _labScriptPrefab, labScript, this.transform.GetChild(1));
    }

    public void ActivateInventory()
    {
        ActivateModal(createdModalsDict, "inventory", _inventoryPrefab, inventory, this.transform.GetChild(2));
    }

    public void ActivateIntroductorySpiel()
    {
        ActivateModal(createdModalsDict, "introSpiel", _introSpielPrefab, introSpiel, this.transform.GetChild(0));
    }

    public void ActivateModesTutorial()
    {
        ActivateModal(createdModalsDict, "modesTutorial", _modesTutorial, modesTutorial, this.transform.GetChild(0));
    }

    public void ActivateSetUpTutorial()
    {
        UIController.Instance.calibrateClick();
        ActivateModal(createdModalsDict, "setUpTutorial", _setUpTutorial, setUpTutorial, this.transform.GetChild(0));
    }

    public void ActivateMeasureTutorial()
    {
        UIController.Instance.measureClick();
        ActivateModal(createdModalsDict, "measureTutorial", _measureTutorial, measureTutorial, this.transform.GetChild(0));
    }

    public void ActivateExploreTutorial()
    {
        UIController.Instance.exploreClick();
        ActivateModal(createdModalsDict, "exploreTutorial", _exploreTutorial, exploreTutorial, this.transform.GetChild(0));
    }

    public void ActivateTakeDataTutorial()
    {
        UIController.Instance.dataClickTutorial();
        ActivateModal(createdModalsDict, "takeDataTutorial", _takeDataTutorial, takeDataTutorial, this.transform.GetChild(0));
    }



    public void ChangeStep(bool falseIsPrevious, GameObject currentObject)
    {
        currentObject.SetActive(false);

        if (currentObject.name == "IntroSpiel(Clone)")
        {
            ActivateModesTutorial();
        }

        else if (currentObject.name == "ModesTutorial(Clone)")
        {
            if (falseIsPrevious == false) ActivateIntroductorySpiel();
            else ActivateSetUpTutorial();
        }

        else if (currentObject.name == "SetUpToolsTutorial(Clone)")
        {
            if (falseIsPrevious == false) ActivateModesTutorial();
            else ActivateMeasureTutorial();
        }

        else if (currentObject.name == "MeasureToolsTutorial(Clone)")
        {
            if (falseIsPrevious == false) ActivateSetUpTutorial();
            else ActivateExploreTutorial();
        }

        else if (currentObject.name == "ExploreToolsTutorial(Clone)")
        {
            if (falseIsPrevious == false) ActivateMeasureTutorial();
            else ActivateTakeDataTutorial();
        }

        else if (currentObject.name == "TakeDataToolsTutorial(Clone)")
        {
            if (falseIsPrevious == false) ActivateExploreTutorial();
        }
    }
}