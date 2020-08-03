using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    /*

    Camera Manager.
    Keyboard shortcuts:
    • Arrows for translational movement along XY axis
    • AD for X rotation
    • SW for Y rotation
    • QE for Zoom
    • IO for max zoom in - max zoom out

    */

    [Header("Camera Positioning")]
    public Vector2 cameraOffset = new Vector2(10f, -3f);
    public float lookAtOffset = -1f;

    [Header("Move Controls")]
    public float upDownSpeed = 5f;
    public float leftRightSpeed = 5f;
    public float rotateSpeed = 45f;

    [Header("Move Bounds (Set to OpticalBoard Mesh")]
    public Vector2 minBounds, maxBounds;
    [Header("Zoom Controls")]
    public float zoomSpeed = 4f;
    public float nearZoomLimit = 0.5f;
    public float farZoomLimit = 30f;
    public float startingZoom = 5f;

    IZoom zoom;
    Vector3 frameMove;
    float frameRotateX;
    float frameRotateY;
    float frameZoom;
    //bool home;
    Camera cam;

    private void Awake()
    {
        cam = GetComponentInChildren<Camera>();
        cam.transform.localPosition = new Vector3(0f, Mathf.Abs(cameraOffset.y), -Mathf.Abs(cameraOffset.x));
        zoom = new OrhtographicZoom(cam, startingZoom);
        cam.transform.LookAt(transform.position + Vector3.up * lookAtOffset);
    }
    
    private void OnEnable()
    {
        KeyboardInputManager.OnMoveInput += UpdateFrameMove;
        KeyboardInputManager.OnRotateXInput += UpdateFrameRotateX;
        KeyboardInputManager.OnRotateYInput += UpdateFrameRotateY;
        KeyboardInputManager.OnZoomInput += UpdateFrameZoom;
        //MouseInputManager.OnMoveInput += UpdateFrameMove;
        //MouseInputManager.OnRotateInput += UpdateFrameRotate;
        //MouseInputManager.OnZoomInput += UpdateFrameZoom;

    }
    private void OnDisable()
    {
        KeyboardInputManager.OnMoveInput -= UpdateFrameMove;
        KeyboardInputManager.OnRotateXInput -= UpdateFrameRotateX;
        KeyboardInputManager.OnRotateYInput -= UpdateFrameRotateY;
        KeyboardInputManager.OnZoomInput -= UpdateFrameZoom;
        //MouseInputManager.OnMoveInput -= UpdateFrameMove;
        //MouseInputManager.OnRotateInput -= UpdateFrameRotate;
        //MouseInputManager.OnZoomInput -= UpdateFrameZoom;

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

    private void LateUpdate()
    {
        /*if (home == true)
        {
            cam = GetComponentInChildren<Camera>();
            cam.transform.localPosition = new Vector3(0f, Mathf.Abs(cameraOffset.y), -Mathf.Abs(cameraOffset.x));
            zoom = new OrhtographicZoom(cam, startingZoom);
        }*/
        if (frameMove != Vector3.zero)
        {
            Vector3 speedModFrameMove = new Vector3(frameMove.x * leftRightSpeed, frameMove.y * upDownSpeed, frameMove.z);
            transform.position += transform.TransformDirection(speedModFrameMove) * Time.deltaTime;
            LockPositionInBounds();
            frameMove = Vector3.zero;
        }

        if (frameRotateX != 0f)
        {
            transform.Rotate(Vector3.up, frameRotateX * Time.deltaTime * rotateSpeed);
            frameRotateX = 0f;
        }

        if (frameRotateY != 0f)
        {
            transform.Rotate(Vector3.right, frameRotateY * Time.deltaTime * rotateSpeed);
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
            zoom = new OrhtographicZoom(cam, nearZoomLimit);
        } else if (frameZoom == -100f)
        {
            zoom = new OrhtographicZoom(cam, farZoomLimit);
        }

    }

    private void LockPositionInBounds()
    {
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x),
            transform.position.y,
            Mathf.Clamp(transform.position.z, minBounds.y, maxBounds.y)
        );
    }
}
