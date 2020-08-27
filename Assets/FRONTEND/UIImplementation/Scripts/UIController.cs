using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum Mode
{
    Calibrate,
    Measure,
    Explore,
    DataTake
}

public class UIController : MonoBehaviour
{
    private static UIController _instance;
    public static UIController Instance
    {   get
    {
            if (_instance == null)
                Debug.LogError("UIController is NULL.");

            return _instance;

        }
    }
    
    private void Awake()
    {
        _instance = this; 
    }
    
    public TooltrayController tooltray;
    public GameObject emailManager;
    public Mode currentMode;
    [Header("Mode Buttons")]
    public Button calibrate;
    public Button measure;
    public Button explore;
    public Button data;
    public Camera mainCam, screenCam;
    public GameObject inputManager;

    [Header("Mode Tabs")]
    [SerializeField]
    private GameObject calibrateTab;
    [SerializeField]
    private GameObject measureTab;
    [SerializeField]
    private GameObject exploreTab;
    [SerializeField]
    private GameObject dataTab;

    void Start()
    {
        emailManager = GameObject.Find("v1 EmailManager");
        emailManager.SetActive(false);

        screenCam.enabled = false;

        ModalManager.Instance.ActivateIntroductorySpiel();
        //calibrateClick();        //by default launch into this mode (change this for intro mode in the future)

        calibrate.onClick.AddListener(calibrateClick);
        measure.onClick.AddListener(measureClick);
        explore.onClick.AddListener(exploreClick);
        data.onClick.AddListener(dataClick);
    }

    public void calibrateClick()
    {
        DeactivateTabs();
        calibrateTab.SetActive(true);
        currentMode = Mode.Calibrate;
        tooltray.SetTrayContents(currentMode);
        Debug.Log("calibrate");
    }

    public void measureClick()
    {
        DeactivateTabs();
        measureTab.SetActive(true);
        currentMode = Mode.Measure;
        tooltray.SetTrayContents(currentMode);
        Debug.Log("Measure");
    }

    public void exploreClick()
    {
        DeactivateTabs();
        exploreTab.SetActive(true);
        currentMode = Mode.Explore;
        tooltray.SetTrayContents(currentMode);
        Debug.Log("Explore");
    }

    public void dataClick()
    {
        DeactivateTabs();
        dataTab.SetActive(true);
        currentMode = Mode.DataTake;
        tooltray.SetTrayContents(currentMode);
        Debug.Log("Data");

        DetectorBehaviour detectorBehaviour = GameObject.Find("v2 DETECTOR").GetComponent<DetectorBehaviour>();
        detectorBehaviour.SetExternalRefs();
        detectorBehaviour.LoadScreen();
    }

    public void dataClickTutorial()
    {
        DeactivateTabs();
        dataTab.SetActive(true);
        currentMode = Mode.DataTake;
        tooltray.SetTrayContents(currentMode);
        Debug.Log("Data");
    }

        private void DeactivateTabs()
    {
        calibrateTab.SetActive(false);
        measureTab.SetActive(false);
        exploreTab.SetActive(false);
        dataTab.SetActive(false);
    }

    public void SwitchCams(bool screenOn)
    {
        mainCam.enabled = !screenOn;
        screenCam.enabled = screenOn;

        if (!screenOn)
        {
            mainCam.GetComponentInParent<CameraManager>().ResetCamera();
        }
    }



    public void isActiveInputManager(bool isActive)
    {
        inputManager.SetActive(isActive);
    }

}