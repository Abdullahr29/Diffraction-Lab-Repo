using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputManager : MonoBehaviour
{
    public delegate void MoveInputHandler(Vector3 moveVector);
    public delegate void RotateXInputHandler(float rotateXAmount);
    public delegate void RotateYInputHandler(float rotateYAmount);
    public delegate void RotateZInputHandler(float rotateZAmount);
    public delegate void ZoomInputHandler(float zoomAmount);
    //public delegate void HomeInputHandler(bool home);
}
