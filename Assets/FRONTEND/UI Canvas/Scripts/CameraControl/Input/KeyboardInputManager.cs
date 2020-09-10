using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInputManager : InputManager
{
    public static event MoveXInputHandler OnMoveXInput;
    public static event MoveYInputHandler OnMoveYInput;
    public static event RotateXInputHandler OnRotateXInput;
    public static event RotateYInputHandler OnRotateYInput;
    public static event ZoomInputHandler OnZoomInput;
    public static event HomeInputHandler OnHomeInput;

    void Update()
    {
        // Move X

        if (Input.GetKey(KeyCode.A)) {
            OnMoveXInput?.Invoke(Vector3.left);
        }
        if (Input.GetKey(KeyCode.D)) {
            OnMoveXInput?.Invoke(Vector3.right);
        }

        // Move Y
        if (Input.GetKey(KeyCode.Q))
        {
            OnMoveYInput?.Invoke(Vector3.up);
        }
        if (Input.GetKey(KeyCode.E))
        {
            OnMoveYInput?.Invoke(Vector3.down);
        }

        // Move/Zoom into Z axis
        if (Input.GetKey(KeyCode.W)) {
            OnZoomInput?.Invoke(-1f);
        }
        if (Input.GetKey(KeyCode.S)) {
            OnZoomInput?.Invoke(1f);
        }

        // Rotate horizontal 
        if (Input.GetKey(KeyCode.LeftArrow)) {
            OnRotateXInput?.Invoke(1f);
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            OnRotateXInput?.Invoke(-1f);
        }

        // Rotate vertical
        if (Input.GetKey(KeyCode.DownArrow)) {
            OnRotateYInput?.Invoke(-1f);
        }
        if (Input.GetKey(KeyCode.UpArrow)) {
            OnRotateYInput?.Invoke(1f);
        }

        // Max Zoom
        /*if (Input.GetKey(KeyCode.Q)) {
            OnZoomInput?.Invoke(-100f);
        }*/
        if (Input.GetKey(KeyCode.O)) {
            OnZoomInput?.Invoke(100f);
        }

        //Home position -- need to finish this
        // LM 10/09/20 - Currently breaking the screen cam unless we create a different camera controller script
        //if (Input.GetKey(KeyCode.H)) {
        //    OnHomeInput?.Invoke(1f);
        //}
    }
}
