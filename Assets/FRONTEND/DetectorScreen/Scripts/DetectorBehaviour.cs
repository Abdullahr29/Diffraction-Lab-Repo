using UnityEngine;
using UnityEngine.UI;

// Simple Script that is referenced externally by the Grating
// Acts as a simple interface and connects to the internal display script

public class DetectorBehaviour : MonoBehaviour
{
    [Header("EXTRERNAL REFERENCES")]
    public GratingBehaviour grating;
    public GameObject CMOS;
   // public GameObject emailManager;

    [Header("INTERNAL REFERENCES")]
    public DetectorDisplayScript display;
    public GameObject screen;
    public Image IMG;

    

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
        Debug.Log("Load Button pressed");
        float projectedScreenSize;
        float[,] matrix;
        grating.GetMatrix(out projectedScreenSize, out matrix);  //Need to split GetMatrix into GetClean, then after we locate maxima can call GetDirty
        Fill(matrix, projectedScreenSize);
        //emailManager.SetActive(true);
    }

    // Function called when the InputMatrix needs to be displayed.
    // Handles the entire flow of information through the detecor.
    public void Fill(float[,] InputMatrix, float projectedScreenSize)
    {
        // 1. Fetch screen parameters
        resolution = InputMatrix.GetLength(0);

        // 2. Convert the inputMatrix to RGB
        // currently not needed
        //rgbMatrix = ConvertToRGB(inputMatrix);
        Debug.Log(InputMatrix.GetLength(0));
        //convertedMatrix = ConvertToRealSpace(InputMatrix, projectedScreenSize);//needs to be fixed
        //convertedMatrix = inputMatrix;

        // 3. Display the rgbMatrix to the screen
        display.Display(InputMatrix, IMG);//should be converted matrix eventually
    }

    // Function that takes the Input Matrix and outputs a subset depending on the screen parameters
    private float[,] ConvertToRealSpace(float[,] InputMatrix, float projectedScreenSize)
    {
        //put in the grating script from here
        
        //up to here
        // -- declare the outputMatrix array
        float[,] outputMatrix;
        
        // BRANCH -- Check if the matrix fits in the screen
        if (projectedScreenSize > screenWidth)
        {
            // the screen is smaller than necessary 
            // we must take a subset of the original matrix and display that
            Debug.Log("PROJECTION > SCREEN SIZE - TAKING SUBSET OF MATRIX");

            // -- From the screen size, and the distance Per pixel, calculate how many pixels should be displayed
            float pixelsToDisplayFLOAT = screenWidth / (projectedScreenSize/inputMatrix.GetLength(0));
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

    

    public void SetExternalRefs()
        //Used as references to the prefabs were causing errors, instead we shall get the ext references from the current scene
    {
        grating = ObjectManager.Instance.Grating.GetComponent<GratingBehaviour>();
        CMOS = ObjectManager.Instance.Cmos;
       // emailManager = GameObject.Find("v1 EmailManager (Initially Disabled)");
    }
}
