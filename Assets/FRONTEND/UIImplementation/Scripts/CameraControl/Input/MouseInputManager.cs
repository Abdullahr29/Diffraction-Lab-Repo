using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputManager : InputManager
{
    Vector2Int screen;
    float mouseXStart;
    float mouseYStart;
    public static event MoveInputHandler OnMoveInput;
    public static event RotateXInputHandler OnRotateXInput;
    public static event RotateYInputHandler OnRotateYInput;
    public static event ZoomInputHandler OnZoomInput;

    private void Awake()
    {
        screen = new Vector2Int(Screen.width, Screen.height);
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // rotation
        /*if (Input.GetMouseButtonDown(0)) {
            mouseXStart = mouseX;
            mouseYStart = mouseY;
        }
        else if (Input.GetMouseButton(0))
        {
            if (mouseX < mouseXStart)
            {
                OnRotateXInput?.Invoke(-1f);
            }
            else if (mouseX > mouseXStart)
            {
                OnRotateXInput?.Invoke(1f);
            }

            if (mouseY < mouseYStart)
            {
                OnRotateYInput?.Invoke(1f);
            }
            else if (mouseY > mouseYStart)
            {
                OnRotateYInput?.Invoke(-1f);
            }
        }*/

        // zoom
        if (Input.mouseScrollDelta.y > 0)
        {
            OnZoomInput?.Invoke(-3f);
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            OnZoomInput?.Invoke(3f);
        }
    }
}
