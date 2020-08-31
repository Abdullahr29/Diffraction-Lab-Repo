using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputManager : MonoBehaviour
{
    public delegate void MoveInputHandler(Vector3 moveVector);
    public delegate void RotateXInputHandler(float rotateXAmount);
    public delegate void RotateYInputHandler(float rotateZAmount);
    public delegate void ZoomInputHandler(float zoomAmount);
    public delegate void HomeInputHandler(float home);
}
