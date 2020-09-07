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
    public float rotateXSpeed = 45f;
    public float rotateYSpeed = 45f;

    [Header("Bounds")]
    public Vector2 minMoveBounds = new Vector2(-50, -50);
    public Vector2 maxMoveBounds = new Vector2(50, 50);
    public float minRotate = 0f;
    public float maxRotate = 80.0f;

    [Header("Zoom Controls")]
    public float zoomSpeed = 1f;
    public float nearZoomLimit = 0.2f;
    public float farZoomLimit = 2f;
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

    Vector3 camPosition;
    RaycastHit hit;
    float rayLength = 0.1f;

    public LayerMask IgnoreMe;
    List<Vector3> allDirections;
    List<Vector3> basicDirections;
    List<Vector3> reverseDirections;

    public Vector3 lastPos, lastLocal;
    public Quaternion lastRot;


    private void Awake()
    {
        cam = GetComponentInChildren<Camera>();
        cam.transform.localPosition = new Vector3(0f, 0f, -Mathf.Abs(cameraOffset.x));
        Vector3 rot = transform.eulerAngles;
        rot.x = 0.1f;
        transform.eulerAngles = rot;
        zoom = (IZoom)new PerspectiveZoom(cam, cameraOffset, startingZoom);

        addOtherDirections();
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
        Vector3 oldPosition = transform.position;
        Quaternion oldRotation = transform.rotation;

        if (homeClick != 0f)
        {
            ResetCamera();
            homeClick = 0f;
        }

        if (frameMove != Vector3.zero)
        {
            Vector3 speedModFrameMove = new Vector3(frameMove.x * leftRightSpeed, frameMove.y, frameMove.z);
            transform.position += transform.TransformDirection(speedModFrameMove) * Time.deltaTime;

            if (IsColliding() == true)
            {
                transform.position = oldPosition;
            }

            frameMove = Vector3.zero;
        }

        if (frameRotateX != 0f)
        {
            Vector3 rot = transform.eulerAngles;
            rot.y += frameRotateX * Time.deltaTime * rotateXSpeed;
            transform.eulerAngles = rot;
            
            if (IsColliding() == true)
            {
                rot.y -= frameRotateX * Time.deltaTime * rotateXSpeed;
                transform.eulerAngles = rot;
            }

            frameRotateX = 0f;
        }

        if (frameRotateY != 0f)
        {
            Vector3 rot = transform.eulerAngles;
            float newRot = rot.x + frameRotateY * Time.deltaTime * rotateYSpeed;

            if (newRot < 90 && newRot > 0)
            {
                rot.x += frameRotateY * Time.deltaTime * rotateYSpeed;
                transform.eulerAngles = rot;

                if (IsColliding() == true)
                {
                    rot.x -= frameRotateY * Time.deltaTime * rotateYSpeed;
                    transform.eulerAngles = rot;
                }
            }

            frameRotateY = 0f;
        }

        if (frameZoom < 0f)
        {
            zoom.ZoomIn(cam, Time.deltaTime * Mathf.Abs(frameZoom) * zoomSpeed, nearZoomLimit);

            if (IsColliding() == true)
            {
                zoom.ZoomOut(cam, -Time.deltaTime * -frameZoom * zoomSpeed, farZoomLimit);
            }

            frameZoom = 0f;
        }
        else if (frameZoom > 0f)
        {
            zoom.ZoomOut(cam, -Time.deltaTime * frameZoom * zoomSpeed, farZoomLimit);
            frameZoom = 0f;
        }

        /*else if (frameZoom == -100f)
        {
            zoom.ZoomInMax(cam, nearZoomLimit);

            while (IsColliding() == true)
            {
                zoom.ZoomOut(cam, -Time.deltaTime * frameZoom * zoomSpeed, farZoomLimit);
            }

        }*/

        else if (frameZoom == -100f)
        {
            zoom.ZoomOutMax(cam, farZoomLimit);
        }
    }

    public void ResetCamera(bool loadLast = false)
    {
        //loadLast to be used if we wish to restore some previously stored camera position
        if (loadLast && lastPos != null)
        {
            transform.position = lastPos;
            transform.rotation = lastRot;
            cam.transform.localPosition = lastLocal;
        }
        else
        {
            transform.position = new Vector3(0f, 0f, 0f);
            transform.rotation = Quaternion.identity;
            cam.transform.localPosition = new Vector3(0f, Mathf.Abs(cameraOffset.y), -Mathf.Abs(cameraOffset.x));
            zoom = (IZoom)new PerspectiveZoom(cam, cameraOffset, startingZoom);
        }        
    }

    public void OnDataEnter()
    {
        lastPos = this.transform.position;
        lastRot = this.transform.rotation;
        lastLocal = cam.transform.localPosition;
        ResetCamera();
    }


    private bool IsColliding()
    {
        bool isHit = false;
        camPosition = cam.transform.position;

        foreach (Vector3 direction in allDirections)
        {
            Ray ray = new Ray(camPosition, direction);
            Debug.DrawRay(camPosition, direction, Color.yellow);
            if (Physics.Raycast(ray, out hit, rayLength, ~IgnoreMe))
            {
                isHit = true;
            }
        }

        return isHit;
    }

    private void addOtherDirections()
    {

        basicDirections = new List<Vector3> { cam.transform.up, cam.transform.right, cam.transform.forward };

        foreach(Vector3 direction in basicDirections.ToArray())
        {
            if (basicDirections.Contains(-direction) != true)
            {
                basicDirections.Add(-direction);
            }
        }

        allDirections = basicDirections;
        reverseDirections = basicDirections;
        reverseDirections.Reverse();

        foreach(Vector3 d1 in basicDirections.ToArray())
        {
            foreach(Vector3 d2 in reverseDirections.ToArray())
            {
                if (d1 != d2)
                {
                    allDirections.Add((d1 + d2).normalized);
                }
            }
        }
    }
}

