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
    public GameObject inputManager, cameraManager;


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
        data.gameObject.SetActive(false);

        emailManager = ObjectManager.Instance.EmailManager;
        emailManager.SetActive(false);

        screenCam = ObjectManager.Instance.ScreenCam.GetComponent<Camera>();
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
        cameraManager.GetComponent<CameraManager>().OnDataEnter();
        DeactivateTabs();
        dataTab.SetActive(true);
        currentMode = Mode.DataTake;
        tooltray.SetTrayContents(currentMode);
        isActiveInputManager(false);
        Debug.Log("Data");

        DetectorBehaviour detectorBehaviour = ObjectManager.Instance.Screen.GetComponent<DetectorBehaviour>();
        CollideWithLaser(false);

        if (ObjectManager.Instance.Grating != null & ObjectManager.Instance.Cmos != null)
        {
            detectorBehaviour.SetExternalRefs();
            detectorBehaviour.LoadScreen();
        }
        if (ObjectManager.Instance.Laser != null)
        {
            ObjectManager.Instance.PropagationManager.SetActive(true);
            ObjectManager.Instance.Laser.GetComponent<LaserBehaviour>().ActivateLaser();
        }                        
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
        isActiveInputManager(true);

        if (ObjectManager.Instance.Laser != null && currentMode == Mode.DataTake)
        {
            ObjectManager.Instance.Laser.GetComponent<LaserBehaviour>().DeactivateLaser();
            CollideWithLaser(true);
        }

        if (currentMode == Mode.DataTake && ObjectManager.Instance.Board != null) //SHAKEY FIX - Used to check if we are in tutorial, replace with narative manager ref
        {
            cameraManager.GetComponent<CameraManager>().ResetCamera(true);  //If we a switching out of take data mode then revert to last saved camera transform
        }
        
    }

    public void SwitchCams(bool screenOn)
    {
        mainCam.enabled = !screenOn;
        screenCam.enabled = screenOn;
    }



    public void isActiveInputManager(bool isActive)
    {
        inputManager.SetActive(isActive);
    }

    void CollideWithLaser(bool status)
    {
        ObjectManager.Instance.Grating.transform.Find("DiffrMountSimplified").Find("DiffrMount").GetComponent<MeshCollider>().enabled = status;
        ObjectManager.Instance.Lens.transform.Find("LensMountSimplified").Find("LensMount").GetComponent<MeshCollider>().enabled = status;
    }
}
