using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    private static ObjectManager _instance;

    /* To add a new type of object, explicitly define a full property for the object and add to
     * the relevant list in Start() */ 


    /*Declare private serialized gameobject for each component to be managed
     * For objects present in the scene before runtime link these in the inspector
     * otherwise when instantiated this script will assign a reference to the correct variable */

    //[Header("Cameras")]
    [SerializeField]
    private GameObject mainCam, screenCam;

    //[Header("Optical Components")]
    [SerializeField]
    private GameObject board, laser, lens, grating, screen, cmos;

    //[Header("Managers")]
    [SerializeField]
    private GameObject propagationManager, emailManager, measureController;

    //[Header("Object Lists")]
    private List<GameObject> cameraList, componentList, managerList;
    private Dictionary<obj, GameObject> activeObjects;

    //[Header("Counters")]
    public int numCameras, numComponents, numManagers, numActive;


    //Property definitions (done so fields can remain serializable)
    public GameObject MainCam
    {
        get { return mainCam; }
        set 
        { 
            bool isNewValue = CheckUpdateCounter(mainCam, value);
            mainCam = value;
            if (isNewValue)
            {
                obj key = obj.camMain;
                UpdateActive(key, value);                
                UpdateCounters();
            }
        }
    }
    public GameObject ScreenCam
    {
        get { return screenCam; }
        set
        {
            bool isNewValue = CheckUpdateCounter(screenCam, value);            
            screenCam = value;
            if (isNewValue)
            {
                obj key = obj.camScreen;
                UpdateActive(key, value);
                UpdateCounters();
            }
        }
    }
    public GameObject Board
    {
        get { return board; }
        set
        {
            bool isNewValue = CheckUpdateCounter(board, value);
            board = value;
            if (isNewValue)
            {
                obj key = obj.board;
                UpdateActive(key, value);
                UpdateCounters();
            }
        }
    }
    public GameObject Laser
    {
        get { return laser; }
        set
        {
            bool isNewValue = CheckUpdateCounter(laser, value);
            laser = value;
            if (isNewValue)
            {
                obj key = obj.laser;
                UpdateActive(key, value);
                UpdateCounters();
            }
        }
    }

    public GameObject Lens
    {
        get { return lens; }
        set
        {
            bool isNewValue = CheckUpdateCounter(lens, value);
            lens = value;
            if (isNewValue)
            {
                obj key = obj.lens;
                UpdateActive(key, value);
                UpdateCounters();
            }
        }
    }
    public GameObject Grating
    {
        get { return grating; }
        set
        {
            bool isNewValue = CheckUpdateCounter(grating, value);
            grating = value;
            if (isNewValue)
            {
                obj key = obj.grating;
                UpdateActive(key, value);
                UpdateCounters();
            }
        }
    }
    public GameObject Screen
    {
        get { return screen; }
        set
        {
            bool isNewValue = CheckUpdateCounter(screen, value);
            screen = value;
            if (isNewValue)
            {
                obj key = obj.screen;
                UpdateActive(key, value);
                UpdateCounters();
            }
        }
    }
    public GameObject Cmos
    {
        get { return cmos; }
        set
        {
            bool isNewValue = CheckUpdateCounter(cmos, value);
            cmos = value;
            if (isNewValue)
            {
                obj key = obj.cmos;
                UpdateActive(key, value);
                UpdateCounters();
            }
        }
    }
    public GameObject PropagationManager
    {
        get { return propagationManager; }
        set
        {
            bool isNewValue = CheckUpdateCounter(propagationManager, value);
            propagationManager = value;
            if (isNewValue)
            {
                obj key = obj.propagation;
                UpdateActive(key, value);
                UpdateCounters();
            }
        }
    }
    public GameObject EmailManager
    {
        get { return emailManager; }
        set
        {
            bool isNewValue = CheckUpdateCounter(emailManager, value);
            emailManager = value;
            if (isNewValue)
            {
                obj key = obj.email;
                UpdateActive(key, value);
                UpdateCounters();
            }
        }
    }
    public GameObject MeasureController
    {
        get { return measureController; }
        set
        {
            bool isNewValue = CheckUpdateCounter(measureController, value);
            measureController = value;
            if (isNewValue)
            {
                obj key = obj.measure;
                UpdateActive(key, value);
                UpdateCounters();
            }
        }
    }

    //Object manager as singleton
    public static ObjectManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("ObjectManager is NULL.");                
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;   
    }


    // Start is called before the first frame update
    void Start()
    {
        cameraList = new List<GameObject> { mainCam, screenCam };
        componentList = new List<GameObject> { board, laser, lens, grating, screen, cmos };
        managerList = new List<GameObject> { propagationManager, emailManager, measureController };

        UpdateCounters();        
    }

    void UpdateCameraRefs()
    {
        //Grabs the main camera through the Camera class - all others by name
        mainCam = Camera.main.gameObject;
        screenCam = GameObject.Find("ScreenCam");

        numCameras = 2;
    }

    void UpdateActive(obj key, GameObject value)
    {
        if (activeObjects.ContainsKey(key) ^ value != null)
        {
            if (value == null)
            {
                activeObjects.Remove(key);
            }
            else
            {
                activeObjects.Add(key, value);
            }
        }
    }

    Boolean CheckUpdateCounter(GameObject property, GameObject value)
    {
        if (value == null ^ property == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void UpdateCounters()
    {
        //Updates counters by determining the status of the appropriate variables

        List<GameObject>[] tempObj = { cameraList, componentList, managerList };
        int[] tempCountArray = { 0, 0, 0 };
        //int[] tempCountArray = { numCameras, numComponents, numManagers };
        numActive = 0;
        activeObjects = null;

        //Loop through the elements of each temporary array above
        for (int i = 0; i < tempObj.Length; i++)
        {
            int tempCount = 0;

            //loop through list of gameobjects tempObj[i], where item is a gameobject
            foreach (var item in tempObj[i])
            {
                if (item != null)
                {
                    tempCount += 1;
                    if (item.activeInHierarchy)
                    {
                        numActive += 1;
                    }
                }                
            }

            tempCountArray[i] = tempCount;
        }

        numCameras = tempCountArray[0];
        numComponents = tempCountArray[1];
        numManagers = tempCountArray[2];
    }    
}
