using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInputManager : InputManager
{
    public static event MoveInputHandler OnMoveInput;
    public static event RotateXInputHandler OnRotateXInput;
    public static event RotateYInputHandler OnRotateYInput;
    public static event ZoomInputHandler OnZoomInput;
    //public static event HomeInputHandler OnHomeInput;

    void Update()
    {
        // Move
        if (Input.GetKey(KeyCode.UpArrow)) {
            OnMoveInput?.Invoke(Vector3.up);
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            OnMoveInput?.Invoke(-Vector3.up);
        }

        if (Input.GetKey(KeyCode.LeftArrow)) {
            OnMoveInput?.Invoke(-Vector3.right);
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            OnMoveInput?.Invoke(Vector3.right);
        }

        // Rotate 
        if (Input.GetKey(KeyCode.A)) {
            OnRotateXInput?.Invoke(-1f);
        }
        if (Input.GetKey(KeyCode.D)) {
            OnRotateXInput?.Invoke(1f);
        }

        // Rotate 
        if (Input.GetKey(KeyCode.S)) {
            OnRotateYInput?.Invoke(-1f);
        }
        if (Input.GetKey(KeyCode.W)) {
            OnRotateYInput?.Invoke(1f);
        }

        // Zoom
        if (Input.GetKey(KeyCode.Q)) {
            OnZoomInput?.Invoke(-1f);
        }
        if (Input.GetKey(KeyCode.E)) {
            OnZoomInput?.Invoke(1f);
        }

        // Max Zoom
        if (Input.GetKey(KeyCode.I)) {
            OnZoomInput?.Invoke(-100f);
        }
        if (Input.GetKey(KeyCode.O)) {
            OnZoomInput?.Invoke(100f);
        }

        // Home position -- need to finish this
        /*if (Input.GetKey(KeyCode.H)) {
            OnHomeInput?.Invoke(true);
        }*/
    }
}
