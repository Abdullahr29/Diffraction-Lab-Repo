using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Script to handle the display functionality of the detector.
// 1. Takes a normalised inputMatrix (the output of the grating intensity matrix) of values 0-1 corresponding to relative intensity
// 2. Create a 2D texture of the same dimensions
// 3. Loop through all grid locations and populate the texture with colour depending on intensity
// 4. Apply the texture to the Image

public class DetectorDisplayScript : MonoBehaviour
{
    
    [Header("EXTERNAL REFERENCES")]
    public gratingPos gratingPos;
    public GameObject emailManager;

    [Header("INTERNAL REFERENCES")]
    public Image IMG;
    public GameObject screen;
    public Transform screenOrigin;

    
    //float screenDistance;
    float screenHeight;
    float screenWidth;
    int resolution; // size of the square matrix
    Texture2D screenTexture;
    float[,] inputMatrix;
    float[,] rgbMatrix;
    float[,] convertedMatrix;
    Vector3 gratingPosition;
    Vector3 screenPosition;


    // public accessors - used by the measurement controller
    public float ScreenHeight { get { return screenHeight; } }
    public float ScreenWidth { get { return screenWidth; } }
    public float Resolution { get { return resolution; } }
    public float[,] Matrix { get { return inputMatrix; } }


    private void Start()
    {
        //fetch the screenWidth / screenHeight from the current gameobject
        screenWidth = transform.localScale.x;
        screenHeight = transform.localScale.y;
        
    }

    // Function called when the InputMatrix needs to be displayed.
    // Handles the entire flow of information through the detecor.
    public void Fill(float[,] InputMatrix)
    {
        // 1. make local copy of input matrix
        inputMatrix = InputMatrix;

        // 2. Take inputMatrix and decode parameters (resolution)
        resolution = inputMatrix.GetLength(0);

        // 3. Convert the inputMatrix to RGB
        // currently not needed
        //rgbMatrix = ConvertToRGB(inputMatrix);

        convertedMatrix = ConvertToRealSpace(inputMatrix);
        
        // 4. Display the rgbMatrix to the screen
        Display(convertedMatrix);
    }


    // Function that counts the number of pixels between the center of the matrix and the first maximum
    private int PixelCountToFirstMaxima(float[,] InputMatrix)
    {
        // 1. from the matrix properties - find x/y values to scan
        // 2. Scan from the centre of the matrix to the edge until first maxima found
        // 3. return the number of pixels from the center to this first maximum

        int centerPixel = resolution / 2;
        int pixelCount = 0;
        int iterationCount = 0;

        //loop through central row, starting from slightly off center all the way to the edge
        for (int i = centerPixel - 1; i > 0; i--)
        {
            iterationCount++;
            if (InputMatrix[i, centerPixel] > InputMatrix[i + 1, centerPixel] && InputMatrix[i, centerPixel] > InputMatrix[i - 1, centerPixel])
            {
                // if current pixel is brighter than the two neighbouring pixels
                //Debug.Log("local maximum found: i=" + i);
                pixelCount = centerPixel - i;
                break;
            }
        }
        //Debug.Log("Iteration Count: " + iterationCount);
        return pixelCount;
    }

    // Function that takes the Input Matrix and outputs a subset depending on the screen parameters
    private float[,] ConvertToRealSpace(float[,] InputMatrix)
    {
        // 1. From known wavelength, slit parameters + screen distance, calculate theoretical position of first maximum
        
        // -- to be fetched from backend
        float wavelength = (float)670e-9;
        float a = (float)60e-06;

        // -- local calculations
        Vector3 gratingToScreen = transform.position - gratingPos.Pos;
        float D = gratingToScreen.magnitude; //screenDistance
        
        // -- find displacement to 1st maximum (n=1)
        float X = DistanceToNthMaxima(1f, wavelength, a, D);
        Debug.Log("distance to 1st maxima: " + X);

        // 2. From the InputMatrix, calculate the pixel count between the central maximum and first maximum
        int pixelCount = PixelCountToFirstMaxima(InputMatrix);

        // 3. The theoretical distance should correspond to said pixel count.
        float distancePerPixel = X / (float)pixelCount;
        //Debug.Log("Distance per pixel: " + distancePerPixel);

        float projectedScreenSize = distancePerPixel * resolution;
        //Debug.Log("Projected Screen Size: " + projectedScreenSize);
        //Debug.Log("Actual Screen Size: " + screenWidth);

        // -- declare the outputMatrix array
        float[,] outputMatrix;

        // BRANCH -- Check if the matrix fits in the screen
        if (projectedScreenSize > screenWidth)
        {
            // the screen is smaller than necessary 
            // we must take a subset of the original matrix and display that
            //Debug.Log("OUTPUT MATRIX SMALLER THAN INPUT MATRIX");

            // -- From the screen size, and the distance Per pixel, calculate how many pixels should be displayed
            float pixelsToDisplayFLOAT = screenWidth / distancePerPixel;
            //Debug.Log("Pixels to display (FLOAT): " + pixelsToDisplayFLOAT);
            int pixelsToDisplay = (int)pixelsToDisplayFLOAT;
            //Debug.Log("Pixels to display (INT): " + pixelsToDisplay);

            // -- If the output is not even, round it to an even value so that the loop is centered
            if (pixelsToDisplay % 2 != 0)
                pixelsToDisplay++;

            //Debug.Log("Pixels to display (rounded): " + pixelsToDisplay);


            // -- resize the output image appropriately
            screen.transform.localScale = new Vector3(1f, 1f, 1f);


            // -- compute the output matrix by taking a subset of the input matrix
            outputMatrix = DownsizeMatrix(InputMatrix, pixelsToDisplay);
        }
        else
        {
            // projectedScreenSize < screenWidth
            // the screen is larger than necessary
            // display the original matrix in full - scaling the screen appropriately
            //Debug.Log("DETECTOR TOO BIG FOR THE INPUT MATRIX");

            // -- resize the output image appropriately
            screen.transform.localScale = IMG.transform.localScale * projectedScreenSize / screenWidth;

            // -- return the original matrix
            outputMatrix = InputMatrix;   
        }

        return outputMatrix;
    }

    // Function that takes the input matrix, and the dimensions of the matrix to be displayed
    // This is called under the condition that the screen is smaller than the physical size of the full matrix
    private float[,] DownsizeMatrix(float[,] InputMatrix, int pixelsToDisplay)
    {
        // 5. Create a new 2D matrix and populate with only the necessary values
        float[,] outputMatrix = new float[pixelsToDisplay, pixelsToDisplay];

        int lowerBound = resolution / 2 - pixelsToDisplay / 2;
        int upperBound = resolution / 2 + pixelsToDisplay / 2;

        int offset = resolution / 2 - pixelsToDisplay / 2;

        for (int i = lowerBound; i < upperBound; i++)
        {
            for (int j = lowerBound; j < upperBound; j++)
            {
                outputMatrix[i - offset, j - offset] = InputMatrix[i, j];
            }
        }

        return outputMatrix;
    }

    // Function that returns the distance between the central and nth maximum
    // necessary parameters: n - order of maximum, wavelength, a - slit separation, D - screenDistance;
    private float DistanceToNthMaxima(float n, float wavelength, float a, float D)
    {
        // general diffraction grating formula
        // tan(theta) = X / D
        // --> X ~ D * tan(theta)
        // (where, for small angle, tan(theta) ~ sin(theta) ~ theta = n * wavelength / a
        return D * n * wavelength / a;
    }

    // Function that takes the intensity matrix and converts it to RGB
    // currently not being used
    private float[,] ConvertToRGB(float[,] InputMatrix)
    {
        //Currently not needed
        return InputMatrix;
    }

    // Function that takes a 2D array and maps it to a texture
    // This texture is then displayed as an image on the screen
    private void Display(float[,] InputMatrix)
    {
        int resolution = (int)Mathf.Sqrt(InputMatrix.Length);
        screenTexture = new Texture2D(resolution, resolution, TextureFormat.ARGB32, false);
        for (int i = 0; i < resolution; i++)
        {
            for (int j = 0; j < resolution; j++)
            {
                Color pixelColor = new Color(255f, 0f, 0f, InputMatrix[i, j]);
                screenTexture.SetPixel(i, j, pixelColor);
            }
        }

        screenTexture.Apply();

        IMG.GetComponent<Image>().sprite = Sprite.Create(screenTexture, new Rect(0, 0, resolution, resolution), new Vector2(0.5f, 0.5f));
        Debug.Log("Matrix Display Complete");

        emailManager.SetActive(true);
    }
}