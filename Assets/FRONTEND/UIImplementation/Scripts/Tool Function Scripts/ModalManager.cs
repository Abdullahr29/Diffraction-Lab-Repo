using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEditor;

public class ModalManager : MonoBehaviour
{
    // Turn modal manager into singleton
    private static ModalManager _instance;
    public static ModalManager Instance
    {   get
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

    [Header("Parent Transforms")]
    // Parent objects in hirearchy
    [SerializeField]
    private RectTransform _helpLabModals;
    [SerializeField]
    private RectTransform _inventoryParent;
    [SerializeField]
    private RectTransform _introSpielParent;


    [Header("Modal Prefabs")]
    // Find prefabs to instantiate
    [SerializeField]
    private GameObject _helpCalPrefab;
    [SerializeField]
    private GameObject _labScriptPrefab;
    [SerializeField]
    private GameObject _helpMesPrefab;
    [SerializeField]
    private GameObject _helpExplPrefab;
    [SerializeField]
    private GameObject _helpDataPrefab;
    [SerializeField]
    private GameObject _inventoryPrefab;
    [SerializeField]
    private GameObject _introSpielPrefab;

    // Empty game objects to instantiate the prefabs into
    public static GameObject helpCal, helpMes, helpExpl, helpData;
    public static GameObject labScript, inventory, introSpiel;

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

                ActivateModal(createdModalsDict, "calibrateHelp", _helpCalPrefab, helpCal, _helpLabModals);
                break;

            case Mode.Measure:

                ActivateModal(createdModalsDict, "measureHelp", _helpMesPrefab, helpMes, _helpLabModals);
                break;

            case Mode.Explore:

                ActivateModal(createdModalsDict, "exploreHelp", _helpExplPrefab, helpExpl, _helpLabModals);
                break;

            case Mode.DataTake:

                ActivateModal(createdModalsDict, "dataHelp", _helpDataPrefab, helpData, _helpLabModals);
                break;
        }
    }

    public void ActivateLabScript()
    {
        ActivateModal(createdModalsDict, "labScript", _labScriptPrefab, labScript, _helpLabModals);
    }

    public void ActivateInventory()
    {
        ActivateModal(createdModalsDict, "inventory", _inventoryPrefab, inventory, _inventoryParent);
    }

    public void ActivateIntroductorySpiel()
    {
        ActivateModal(createdModalsDict, "introSpiel", _introSpielPrefab, introSpiel, _introSpielParent);
    }
    
}