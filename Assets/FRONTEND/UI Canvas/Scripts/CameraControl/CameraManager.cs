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
    public Vector2 cameraOffset = new Vector2(1f, 0f);

    [Header("Move Controls")]
    public float upDownSpeed = 0.1f;
    public float leftRightSpeed = 0.1f;
    public float rotateXSpeed = 30f;
    public float rotateYSpeed = 30f;

    [Header("Bounds")]
    public Vector2 minMoveBounds = new Vector2(-50, -50);
    public Vector2 maxMoveBounds = new Vector2(50, 50);
    public float minRotate = 0f;
    public float maxRotate = 90.0f;

    [Header("Zoom Controls")]
    public float zoomSpeed = 1f;
    public float nearZoomLimit = 0.4f;
    public float farZoomLimit = 4f;
    public float startingZoom = 2f;

    IZoom zoom;
    Vector3 frameMoveX;
    Vector3 frameMoveY;
    float frameRotateX;
    float frameRotateY;
    float frameZoom;
    float homeClick;
    Vector3 initialPosition;
    IZoom initialZoom;
    Camera cam;

    Vector3 camPosition;
    RaycastHit hit;
    [SerializeField]
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
        KeyboardInputManager.OnMoveXInput += UpdateFrameMoveX;
        KeyboardInputManager.OnMoveYInput += UpdateFrameMoveY;
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
        KeyboardInputManager.OnMoveXInput -= UpdateFrameMoveX;
        KeyboardInputManager.OnMoveYInput -= UpdateFrameMoveY;
        KeyboardInputManager.OnRotateXInput -= UpdateFrameRotateX;
        KeyboardInputManager.OnRotateYInput -= UpdateFrameRotateY;
        KeyboardInputManager.OnZoomInput -= UpdateFrameZoom;
        KeyboardInputManager.OnHomeInput -= UpdateFrameHome;
        MouseInputManager.OnRotateXInput -= UpdateFrameRotateX;
        MouseInputManager.OnRotateYInput -= UpdateFrameRotateY;
        MouseInputManager.OnZoomInput -= UpdateFrameZoom;

    }

    private void UpdateFrameMoveX(Vector3 moveVector)
    {
        frameMoveX += moveVector;
    }
    private void UpdateFrameMoveY(Vector3 moveVector)
    {
        frameMoveY += moveVector;
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

        if (frameMoveX != Vector3.zero)
        {
            Vector3 speedModFrameMove = new Vector3(frameMoveX.x * leftRightSpeed, frameMoveX.y, frameMoveX.z);
            transform.position += transform.TransformDirection(speedModFrameMove) * Time.deltaTime;

            if (IsColliding() == true)
            {
                transform.position = oldPosition;
            }

            frameMoveX = Vector3.zero;
        }

        if (frameMoveY != Vector3.zero)
        {
            Vector3 speedModFrameMove = new Vector3(frameMoveY.x, frameMoveY.y * upDownSpeed, frameMoveY.z);
            transform.position += transform.TransformDirection(speedModFrameMove) * Time.deltaTime;

            if (IsColliding() == true)
            {
                transform.position = oldPosition;
            }

            frameMoveY = Vector3.zero;
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
            Debug.Log(lastPos);
            Debug.Log(lastRot);
            Debug.Log(lastLocal);
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
        Debug.Log(lastLocal);
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

