using System;
using System.IO;
using UnityEngine;

public class GratingBehaviour : MonoBehaviour
{

    public DetectorBehaviour detector;

    [Header("Internal References")]
    public Transform anchor;

    [Header("Grating Parameters")]
    [SerializeField] string file = "Assets/FRONTEND/DetectorScreen/img.txt";

    [Header("PARAMETERS")]
    public float Wavelength = 670e-9f;

    //grating parameters 
    float maxSlitDim = 0.006f;//assuming a square grating 

    //slit parameters 
    public float slitWidth = 0.00006f;
    float slitHeight = 0.0005f;
    float slitSeperation = 0.00012f;

    public float SlitWidth { get { return slitWidth; } }


    public int resolution = 1024;
    [Range(0, 13)] public int resolutionPower;

    float distanceLensScreen;// need to get, 0.1 - 0.8
    float distanceGratingLens;//need to get, 0.25 - 0.5
    float distanceGratingScreen;
    float focalLength = 0.5f;//need to get 
    float angle;//need to get, -30 - 30 
    [Range(0, 5)] public int slits;//need to get

    private int[,] bitmap;
    private float[,] output;//matrix with 2DFFT
    public float[,] temp;


    private Vector3 pos;
    public Vector3 Pos { get { return pos; } }

    public Transform CMOS;
    public GameObject detectorObject;

    // Start is called before the first frame update
    void Start()
    {
        detectorObject = GameObject.Find("v2 DETECTOR");
        pos = anchor.position;
        resolution = Mathf.RoundToInt(Mathf.Pow(2, resolutionPower)); 
        //positionObject();
        bitmap = new int[resolution, resolution];
    }

    public void GetMatrix(out float projectedScreenSize, out float[,] output)
    {
        getParameters();
        pos = anchor.position;
        //make slit bitmap
        //convolve with n slit dirac-delta
        //do FFT
        //lens orientation skew
        //focal length noise + background noise
        //normalise 
        //output data 
        //inverse square 
        //display on screen

        //slit bitmap
        ///slitCalc();not necessary for now
        if (slits == 0) {
            slitHeight = 0.00006f;
        }
        else {
            slitHeight = 0.0005f;
        }

        if (slits > 1) {
            //generating more slits   
            bitmap = Convolve.genNSlits(slits, resolution, slitSeperation, maxSlitDim, slitWidth, slitHeight);
        }
        else {
            //filling matrix otherwise
            fill();
        }
        //doing FFT
        FFT bitMapFFT = new FFT(MatrixCalc.convertToFloat(bitmap));
        bitMapFFT.ForwardFFT();
        output = bitMapFFT.FFTLog;

        //lens skew 
        output = Convolve.biasSkew(output, angle, resolution, slits);

        //detector side stuff here
        // 1. From known wavelength, slit parameters + screen distance, calculate theoretical position of first maximum

        // -- to be fetched from backend
        float wavelength = Wavelength;
        //float a = (float)60e-06;
        float a = this.SlitWidth;

        // -- find distance to screen D
        
        CMOS = detectorObject.GetComponent<DetectorBehaviour>().CMOS.transform;
        Vector3 gratingToScreen = CMOS.position - this.Pos; //CHANGE THIS TRANSFORM TO THAT OF THE CMOS
        gratingToScreen.y = 0;

        float D = gratingToScreen.magnitude; //screenDistance
        //Debug.Log("Distance to Screen: " + D);

        // -- find predicted displacement to 1st maximum (n=1)
        float X = DistanceToNthMaxima(1f, wavelength, a, D);
        //Debug.Log("distance to 1st maximum: " + X);

        // 2. From the InputMatrix, calculate the pixel count between the central maximum and first maximum
        int pixelCount = PixelCountToFirstMaxima(output);
        //Debug.Log("pixel count to 1st maximum: " + pixelCount);

        // 3. The theoretical distance should correspond to said pixel count.
        float distancePerPixel = X / (float)pixelCount;
        //Debug.Log("Distance per pixel: " + distancePerPixel);

        projectedScreenSize = distancePerPixel * resolution;
        //Debug.Log("Projected Screen Size: " + projectedScreenSize);

        //_________________________
        //noise
        noiseLoops(output);

        //normalise
        output = normalise(output);

        //output data
        print(output);

        //inverse square
        ///distanceGratingScreen = distanceGratingLens + distanceLensScreen;
        ///output = Convolve.inverseSquare(output, distanceGratingScreen, resolution);

        //display
        ///output = plotIntensity(output, (resolution/2), resolution);
        ///output = zoom(output, resolution/2, resolution, resolution);//need to implelemnt for 2,3,4,5 slits using Data class

    }

    //normalise data between 0-1
    private float[,] normalise(float[,] mat) {

        //float[,] result = new float[mat.GetLength(0), mat.GetLength(1)];

        int Width = mat.GetLength(0);
        int Height = mat.GetLength(1);
        int i, j;
        float max;

        /*for (i = 0; i <= Width - 1; i++)
            for (j = 0; j <= Height - 1; j++) {
                result[i, j] = (float)Math.Log(1 + mat[i, j]);
            }*/

        max = mat[0, 0];
        for (i = 0; i <= Width - 1; i++)
            for (j = 0; j <= Height - 1; j++) {
                if (mat[i, j] > max)
                    max = mat[i, j];
            }
        for (i = 0; i <= Width - 1; i++)
            for (j = 0; j <= Height - 1; j++) {
                if (mat[i, j] < 0)//may need to be improved, works for now
                    mat[i, j] = 0;
                mat[i, j] = mat[i, j] / max;
            }

        return mat;
    }

    //to create a temporary gameobject in the centre of the grating 
    private void positionObject() {

        Vector3 temp = transform.position;
        //Vector3 temp = transform.parent.position;
        //temp.y = temp.y + 1;

    }

    //creates high res bitmap for grating and slit 
    private void fill()
    {
        float beginSlitWidth, endSlitWidth, beginSlitHeight, endSlitHeight;
        beginSlitWidth = ((maxSlitDim - slitWidth) / 2) * (resolution / maxSlitDim) - 1;
        endSlitWidth = beginSlitWidth + (slitWidth * (resolution / maxSlitDim)) + 1;
        beginSlitHeight = ((maxSlitDim - slitHeight) / 2) * (resolution / maxSlitDim) - 1;
        endSlitHeight = beginSlitHeight + (slitHeight * (resolution / maxSlitDim)) + 1;


        for (int i = (int)beginSlitHeight; i < (int)endSlitHeight; i++)
        {
            for (int j = (int)beginSlitWidth; j < (int)endSlitWidth; j++)
            {
                bitmap[i, j] = 1;
            }
        }
    }

    private void getParameters() {
        GameObject boardObject = ObjectManager.Instance.Board;
        GameObject gratingObject = ObjectManager.Instance.Grating;
        GameObject lensObject = ObjectManager.Instance.Lens;
        GameObject cmosObject = ObjectManager.Instance.Cmos;

        Transform gratingTransform = gratingObject.transform;
        Transform lensTransform = lensObject.transform;
        Transform cmosTransform = cmosObject.transform;

        Vector3 gratingToLens = lensTransform.position - gratingTransform.position;
        gratingToLens.y = 0;
        distanceGratingLens = gratingToLens.magnitude;//takes account of differences in z-axis(across the board) as well not just x-axis(along the board)

        Vector3 lensToScreen = cmosTransform.position - lensTransform.position;
        lensToScreen.y = 0;
        distanceLensScreen = lensToScreen.magnitude;//takes account of differences in z-axis(across the board) as well not just x-axis(along the board)

        distanceGratingScreen = distanceGratingLens + distanceLensScreen;

        angle = lensTransform.rotation.y;
        angle = Math.Abs(angle);
        angle = angle - 90;
        Debug.Log("Angle: " + angle);
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

    //to create noise multiple times
    public void noiseLoops(float[,] output) {//Can be optimised to not use multiple loops 
        int factor = (int)Data.param[slits, 4];//also nLoopFac from Data matrix
        float distance2 = Math.Abs(distanceLensScreen - focalLength);
        int loops = (int)Math.Round((distance2 / focalLength) * factor);

        for (int i = 0; i < loops + 1; i++) {
            Debug.Log("nLoop " + (i + 1));
            output = Convolve.genNoise(normalise(output), distanceLensScreen, focalLength, slits);
        }
    }

    //text outputs
    private void print(float[,] output)
    {
        using (TextWriter tw = new StreamWriter(file))
        {
            for (int i = 0; i < output.GetLength(0); i++)
            {
                for (int j = 0; j < output.GetLength(1); j++)
                {
                    //tw.Write(output[i, j].ToString() + "\t");
                    tw.Write(output[i, j].ToString("#.000") + "\t");
                }
                tw.WriteLine();
            }
        }
        Debug.Log("File saved: " + file);
    }

    //to zoom in for higher slit outputs
    public float[,] zoom(float[,] mat, int row, int col, int size) {
        //row and col are dimensions of the new zoomed in matrix from the centre, must be factors of the resolution
        float[,] temp = new float[row, col];
        int beginRow = (size / 2) - (row / 2);
        int beginCol = (size / 2) - (col / 2);
        for (int i = beginRow; i < beginRow + row; i++) {
            for (int j = beginCol; j < beginCol + col; j++) {
                temp[i - beginRow, j - beginCol] = mat[i, j];
            }
        }
        int rowFac = size / row;
        int colFac = size / col;
        int currCol, currRow;
        for (int i = 0; i < size; i++) {
            currRow = i / rowFac;
            for (int j = 0; j < size; j++) {
                currCol = j / colFac;
                mat[i, j] = temp[currRow, currCol];
            }
        }
        return mat;
    }

    //slit error check
    public void slitCalc() {
        if ((slits == 0) && (slitHeight >= slitWidth * 30)) {
            slits = 1;
        }
        if ((slits == 1) && (slitHeight < slitWidth * 30)) {
            if ((slitWidth * 30) > maxSlitDim) {
                slitHeight = maxSlitDim;
            }
            else {
                slitHeight = slitWidth * 30;
            }
            Debug.Log("Slit height has been increased for single slit diffraction");
        }
        if ((slits > 1) && (slitHeight < slitWidth * 10)) {
            if ((slitWidth * 10) > maxSlitDim) {
                slitHeight = maxSlitDim;
            }
            else {
                slitHeight = slitWidth * 10;
            }
            Debug.Log("Slit height has been increased for double slit diffraction");
        }
    }

    //called to plot the intensity pattern 
    public float[,] plotIntensity(float[,] mat, int row, int res) {
        float[,] plot = new float[res, res];
        float max = 0, min = 100, factor;
        int rowVal;
        for (int i = 0; i < res; i++) {
            if (mat[row, i] > max) {
                max = mat[row, i];
            }
            if (mat[row, i] < min) {
                min = mat[row, i];
            }
        }
        factor = res / max;//if we want to start at 0 then we have to do res/max, if we want to start at the min value we do res/(max - min)
        for (int i = 0; i < res; i++) {//plots the intensity pattern using matrix values
            rowVal = (int)Math.Round(mat[row, i] * factor);
            //rowVal = res - rowVal;
            if (rowVal == res) {
                rowVal--;
            }
            for (int j = -1; j < 2; j++) {
                for (int k = -1; k < 2; k++) {
                    if ((rowVal + j) > -1 && (rowVal + j) < res && (i + k) > -1 && (i + k) < res) {
                        plot[rowVal + j, i + k] = 1;
                    }
                }
            }
        }
        for (int i = 0; i <= (int)max * 10; i++) {//plots an axis on the left of integers
            rowVal = (int)(i * factor / 10) + 1;
            if (rowVal == res) {
                rowVal--;
            }
            if (rowVal > res) {
                rowVal = rowVal - 2;
            }
            for (int j = 1; j < 11; j++) {
                plot[rowVal, j] = 1;
                plot[rowVal - 1, j] = 1;
            }
        }

        return plot;
    }
}
