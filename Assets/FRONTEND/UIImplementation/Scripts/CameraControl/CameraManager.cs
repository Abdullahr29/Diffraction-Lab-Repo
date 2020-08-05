using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    /*

    Camera Manager.
    Keyboard shortcuts:
    • Arrows for translational movement along XY axis
    • AD for rotation about Y
    • SW for rotation about X
    • QE for max Zoom in - max Zoom out
    • H for resetting the initial position and zoom

    • Mouse control for zoom
    • Mouse control for rotate functions but affects move and rotate tool,
      so it is commented out in the MouseInputManager for now

    */

    [Header("Camera Positioning")]
    public Vector2 cameraOffset = new Vector2(3f, 0f);

    [Header("Move Controls")]
    public float upDownSpeed = 1f;
    public float leftRightSpeed = 1f;
    public float rotateSpeed = 45f;

    [Header("Bounds")]
    public Vector2 minMoveBounds = new Vector2(-50,-50);
    public Vector2 maxMoveBounds = new Vector2(50, 50);
    public float minRotate = -15f;
    public float maxRotate = 90.0f;

    [Header("Zoom Controls")]
    public float zoomSpeed = 1f;
    public float nearZoomLimit = 0.8f;
    public float farZoomLimit = 5f;
    public float startingZoom = 2f;

    IZoom zoom;
    Vector3 frameMove;
    float frameRotateX;
    float frameRotateY;
    float frameZoom;
    float homeClick;
    Vector3 initialPosition;
    IZoom initialZoom;
    Camera cam;

    private void Awake()
    {
        cam = GetComponentInChildren<Camera>();
        cam.transform.localPosition = new Vector3(0f, Mathf.Abs(cameraOffset.y), -Mathf.Abs(cameraOffset.x));
        zoom = (IZoom)new PerspectiveZoom(cam, cameraOffset, startingZoom);
    }
    
    private void OnEnable()
    {
        KeyboardInputManager.OnMoveInput += UpdateFrameMove;
        KeyboardInputManager.OnRotateXInput += UpdateFrameRotateX;
        KeyboardInputManager.OnRotateYInput += UpdateFrameRotateY;
        KeyboardInputManager.OnZoomInput += UpdateFrameZoom;
        KeyboardInputManager.OnHomeInput += UpdateFrameHome;
        MouseInputManager.OnRotateXInput += UpdateFrameRotateX;
        MouseInputManager.OnRotateYInput += UpdateFrameRotateY;
        MouseInputManager.OnZoomInput += UpdateFrameZoom;

    }
    private void OnDisable()
    {
        KeyboardInputManager.OnMoveInput -= UpdateFrameMove;
        KeyboardInputManager.OnRotateXInput -= UpdateFrameRotateX;
        KeyboardInputManager.OnRotateYInput -= UpdateFrameRotateY;
        KeyboardInputManager.OnZoomInput -= UpdateFrameZoom;
        KeyboardInputManager.OnHomeInput -= UpdateFrameHome;
        MouseInputManager.OnRotateXInput -= UpdateFrameRotateX;
        MouseInputManager.OnRotateYInput -= UpdateFrameRotateY;
        MouseInputManager.OnZoomInput -= UpdateFrameZoom;

    }

    private void UpdateFrameMove(Vector3 moveVector)
    {
        frameMove += moveVector;
    }
    private void UpdateFrameRotateX(float rotateXAmount)
    {
        frameRotateX += rotateXAmount;
    }
    private void UpdateFrameRotateY(float rotateYAmount)
    {
        frameRotateY += rotateYAmount;
    }
    private void UpdateFrameZoom(float zoomAmount)
    {
        frameZoom += zoomAmount;
    }
    private void UpdateFrameHome(float home)
    {
        homeClick += home;
    }

    private void LateUpdate()
    {
        if (homeClick != 0f)
        {
            ResetCamera();
            homeClick = 0f;
        }

        if (frameMove != Vector3.zero)
        {
            Vector3 speedModFrameMove = new Vector3(frameMove.x * leftRightSpeed, frameMove.y, frameMove.z);
            transform.position += transform.TransformDirection(speedModFrameMove) * Time.deltaTime;
            LockPositionInBounds();
            frameMove = Vector3.zero;
        }

        if (frameRotateX != 0f)
        {
            Debug.Log("rotatedX");
            transform.Rotate(Vector3.up, frameRotateX * Time.deltaTime * rotateSpeed);
            LockYRotationInBounds();
            frameRotateX = 0f;
        }

        if (frameRotateY != 0f)
        {
            transform.Rotate(Vector3.right, frameRotateY * Time.deltaTime * rotateSpeed);
            Quaternion q = transform.rotation;
            LockYRotationInBounds();
            frameRotateY = 0f;
        }

        if (frameZoom < 0f)
        {
            zoom.ZoomIn(cam, Time.deltaTime * Mathf.Abs(frameZoom) * zoomSpeed, nearZoomLimit);
            frameZoom = 0f;
        } else if (frameZoom > 0f)
        {
            zoom.ZoomOut(cam, -Time.deltaTime * frameZoom * zoomSpeed, farZoomLimit);
            frameZoom = 0f;
        } else if (frameZoom == -100f)
        {
            zoom.ZoomInMax(cam, nearZoomLimit);
        } else if (frameZoom == -100f)
        {
            zoom.ZoomOutMax(cam, farZoomLimit);
        }

    }

    private void LockPositionInBounds()
    {
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -50, 50),
            transform.position.y,
            transform.position.z
        );

    }

    private void LockYRotationInBounds()
    {
        Vector3 rot = transform.rotation.eulerAngles;
        rot.x = ClampAngle(rot.x, minRotate, maxRotate);
        rot.z = ClampAngle(rot.z, 0, 0);
        transform.eulerAngles = rot;
    }

    float ClampAngle(float angle, float from, float to)
    {
        // accepts e.g. -80, 80
        if (angle < 0f) angle = 360 + angle;
        if (angle > 180f) return Mathf.Max(angle, 360+from);
        return Mathf.Min(angle, to);
    }

    private void ResetCamera()
    {
        transform.position = new Vector3(0f, 0f, 0f);
        transform.rotation = Quaternion.identity;
        cam.transform.localPosition = new Vector3(0f, Mathf.Abs(cameraOffset.y), -Mathf.Abs(cameraOffset.x));
        zoom = (IZoom)new PerspectiveZoom(cam, cameraOffset, startingZoom);
    }
}
