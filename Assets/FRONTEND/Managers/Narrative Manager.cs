/*
 * Narrative manager is the script in charge of the state of progress of the user, it tells the UI and O&D managers which
 * functionality should be active at which point and stores the data associated with the object positions and orientation
 * to allow the user to be able to undo/re-do at relevant points through the experience. 
 */


using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum State
{
    introduction,
    tutorial,
    main,
}


public class NarrativeManager : MonoBehaviour
{
    private static NarrativeManager _instance;
    public static NarrativeManager Instance
    {
        get
        {
            return _instance;
        }
    }
    private void Awake()
    {
        _instance = this; //not sure why it's throwing this error but I'm not good with singletons,  consult Camilla...
    }

    private bool cameraMvt;
    private bool CameraMvt { get; set; }
    private bool allowTools;
    private bool AllowTools { get; set; }
    private bool undoRedo;
    private bool UndoRedo { get; set; }

    private bool qEngagement;
    private bool QEngagement { get; set; }
    private bool allowModes;
    private bool AllowModes { get; set; }


    public void StateOfSystem(State desiredState)
    {
        switch(desiredState)
        {
            case State.introduction:
                cameraMvt = false;
                allowTools = false;
                undoRedo = false;
                qEngagement = false;
                allowModes = true;
                break;
            case State.tutorial:
                cameraMvt = false;
                allowTools = false;
                undoRedo = true;
                qEngagement = false;
                allowModes = false;
                break;
            case State.main:
                cameraMvt = true;
                allowTools = true;
                undoRedo = true;
                qEngagement = true;
                allowModes = true;
                // need to collect positions when in the main state 
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
