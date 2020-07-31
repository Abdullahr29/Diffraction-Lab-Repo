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
    [Header("INTERNAL REFERENCES")]

    Texture2D screenTexture;

    // Function that takes a 2D array and maps it to a texture
    // This texture is then displayed as an image on the screen
    public void Display(float[,] InputMatrix, Image IMG)
    {
        int resolution = (int)Mathf.Sqrt(InputMatrix.Length);
        screenTexture = new Texture2D(resolution, resolution, TextureFormat.ARGB32, false);
        for (int i = 0; i < resolution; i++)
        {
            for (int j = 0; j < resolution; j++)
            {
                Color pixelColor = new Color(255f, 0f, 0f, InputMatrix[i, j]);
                screenTexture.SetPixel(j, i, pixelColor);//i,j flipped caused error 
            }
        }

        screenTexture.Apply();

        IMG.GetComponent<Image>().sprite = Sprite.Create(screenTexture, new Rect(0, 0, resolution, resolution), new Vector2(0.5f, 0.5f));
        Debug.Log("Matrix Display Complete");

    }
}