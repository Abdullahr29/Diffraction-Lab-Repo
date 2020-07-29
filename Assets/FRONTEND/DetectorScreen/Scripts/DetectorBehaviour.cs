using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simple Script that is referenced externally by the Grating
// Acts as a simple interface and connects to the internal display script

public class DetectorBehaviour : MonoBehaviour
{
    [Header("EXTRERNAL REFERENCES")]
    public IntensityCalcFloat grating;

    DetectorDisplayScript display;
    
    private void Start()
    {
        display = GetComponent<DetectorDisplayScript>();
    }

    public void LoadScreen()
    {
        float[,] inputMatrix = grating.GetMatrix();
        DisplayInterferencePattern(inputMatrix);
    }

    public void DisplayInterferencePattern(float[,] inputMatrix)
    {   
        display.Fill(inputMatrix);
    }


}
