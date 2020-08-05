using UnityEngine;
using UnityEngine.UI;

// Simple Script that is referenced externally by the Grating
// Acts as a simple interface and connects to the internal display script

public class DetectorBehaviour : MonoBehaviour
{
    [Header("EXTRERNAL REFERENCES")]
    public GratingBehaviour grating;
   // public GameObject emailManager;

    [Header("INTERNAL REFERENCES")]
    public DetectorDisplayScript display;
    public GameObject screen;
    public Image IMG;

    [Header("PARAMETERS")]
    public float Wavelength = 670e-9f;

    private float[,] inputMatrix;
    private int resolution;
    private float[,] convertedMatrix;
    private float screenHeight;
    private float screenWidth;


    // public accessors - used by the measurement controller
    public float ScreenHeight { get { return screenHeight; } }
    public float ScreenWidth { get { return screenWidth; } }
    public float Resolution { get { return resolution; } }
    public float[,] Matrix { get { return convertedMatrix; } }


    private void Start()
    {
        //fetch the screenWidth / screenHeight from the current gameobject
        screenWidth = transform.localScale.x;
        screenHeight = transform.localScale.y;
    }

    public void LoadScreen()
    {
        inputMatrix = grating.GetMatrix();
        Fill(inputMatrix);
        //emailManager.SetActive(true);
    }

    // Function called when the InputMatrix needs to be displayed.
    // Handles the entire flow of information through the detecor.
    private void Fill(float[,] InputMatrix)
    {
        // 1. Fetch screen parameters
        resolution = InputMatrix.GetLength(0);

        // 2. Convert the inputMatrix to RGB
        // currently not needed
        //rgbMatrix = ConvertToRGB(inputMatrix);

        convertedMatrix = ConvertToRealSpace(inputMatrix);

        // 3. Display the rgbMatrix to the screen
        display.Display(convertedMatrix, IMG);
    }

    // Function that takes the Input Matrix and outputs a subset depending on the screen parameters
    private float[,] ConvertToRealSpace(float[,] InputMatrix)
    {
        // 1. From known wavelength, slit parameters + screen distance, calculate theoretical position of first maximum

        // -- to be fetched from backend
        float wavelength = Wavelength;
        //float a = (float)60e-06;
        float a = grating.SlitWidth;

        // -- find distance to screen D
        Vector3 gratingToScreen = transform.position - grating.Pos; //CHANGE THIS TRANSFORM TO THAT OF THE CMOS
        gratingToScreen.y = 0;

        float D = gratingToScreen.magnitude; //screenDistance
        Debug.Log("Distance to Screen: " + D);

        // -- find predicted displacement to 1st maximum (n=1)
        float X = DistanceToNthMaxima(1f, wavelength, a, D);
        Debug.Log("distance to 1st maximum: " + X);

        // 2. From the InputMatrix, calculate the pixel count between the central maximum and first maximum
        int pixelCount = PixelCountToFirstMaxima(InputMatrix);
        Debug.Log("pixel count to 1st maximum: " + pixelCount);

        // 3. The theoretical distance should correspond to said pixel count.
        float distancePerPixel = X / (float)pixelCount;
        Debug.Log("Distance per pixel: " + distancePerPixel);

        float projectedScreenSize = distancePerPixel * resolution;
        Debug.Log("Projected Screen Size: " + projectedScreenSize);
        Debug.Log("Actual Screen Size: " + screenWidth);

        // -- declare the outputMatrix array
        float[,] outputMatrix;

        outputMatrix = inputMatrix;

        // BRANCH -- Check if the matrix fits in the screen
        if (projectedScreenSize > screenWidth)
        {
            // the screen is smaller than necessary 
            // we must take a subset of the original matrix and display that
            Debug.Log("PROJECTION > SCREEN SIZE - TAKING SUBSET OF MATRIX");

            // -- From the screen size, and the distance Per pixel, calculate how many pixels should be displayed
            float pixelsToDisplayFLOAT = screenWidth / distancePerPixel;
            Debug.Log("Pixels to display (FLOAT): " + pixelsToDisplayFLOAT);
            int pixelsToDisplay = (int)pixelsToDisplayFLOAT;
            Debug.Log("Pixels to display (ROUNDED): " + pixelsToDisplay);

            // -- If the output is not even, round it to an even value so that the loop is centered
            if (pixelsToDisplay % 2 != 0)
                pixelsToDisplay++;

            Debug.Log("Pixels to display (EVEN): " + pixelsToDisplay);



            // -- resize the output image appropriately
            screen.transform.localScale = new Vector3(1f, 1f, 1f);

            //screenWidth = transform.localScale.x;
            //screenHeight = transform.localScale.y;

            // -- compute the output matrix by taking a subset of the input matrix
            outputMatrix = DownsizeMatrix(InputMatrix, pixelsToDisplay);

            // -- Reset the screen parameters so the measuremenet tool integrates properly
            // resolution (decreased)
            resolution = pixelsToDisplay;

            Debug.Log(" *** SCREEN WIDTH: " + screenWidth * screen.transform.localScale.x);
            Debug.Log(" *** SCREEN HEIGHT: " + screenHeight * screen.transform.localScale.y);
            Debug.Log(" *** RESOLUTION: " + resolution);
        }
        else
        {
            // projectedScreenSize < screenWidth
            // the screen is larger than necessary
            // display the original matrix in full - scaling the screen appropriately
            Debug.Log("PROJECTION < SCREEN SIZE - DOWNSIZING IMAGE");

            // -- resize the output image appropriately
            screen.transform.localScale = IMG.transform.localScale * projectedScreenSize / screenWidth;

            // -- Reset the screen parameters so the measuremenet tool integrates properly
            // resolution - (increased)

            //screenWidth = screen.transform.localScale.x * screenWidth;
            //screenHeight = screen.transform.localScale.y * screenHeight;
            resolution = (int)(InputMatrix.GetLength(0) * screenWidth / projectedScreenSize);

            // -- If the output is not even, round it to an even value so that the loop is centered
            if (resolution % 2 != 0)
                resolution++;

            Debug.Log(" *** SCREEN WIDTH: " + screenWidth);
            Debug.Log(" *** SCREEN HEIGHT: " + screenHeight);
            Debug.Log(" *** RESOLUTION: " + resolution);

            // -- return the original matrix
            outputMatrix = InputMatrix;
        }

        return outputMatrix;
    }

    // Function that counts the number of pixels between the center of the matrix and the first maximum
    private int PixelCountToFirstMaxima(float[,] InputMatrix)
    {
        // 1. from the matrix properties - find x/y values to scan
        // 2. Scan from the centre of the matrix to the edge until first maxima found
        // 3. return the number of pixels from the center to this first maximum

        int centerPixel = resolution / 2;
        int pixelCount = 0;

        //loop through central row, starting from slightly off center all the way to the edge
        for (int i = centerPixel - 1; i > 0; i--)
        {
            //if (InputMatrix[i, centerPixel] > InputMatrix[i + 1, centerPixel] && InputMatrix[i, centerPixel] > InputMatrix[i - 1, centerPixel])
            if (InputMatrix[centerPixel, i] > InputMatrix[centerPixel, i + 1] && InputMatrix[centerPixel, i] > InputMatrix[centerPixel, i - 1])
            {
                // if current pixel is brighter than the two neighbouring pixels
                Debug.Log("local maximum found: i=" + i);

                pixelCount = centerPixel - i;
                break;
            }
        }
        return pixelCount;
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

    public void SetExternalRefs()
        //Used as references to the prefabs were causing errors, instead we shall get the ext references from the current scene
    {
        grating = GameObject.Find("v2 GRATING").GetComponent<GratingBehaviour>();
       // emailManager = GameObject.Find("v1 EmailManager (Initially Disabled)");
    }
}
