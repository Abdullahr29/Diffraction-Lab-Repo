using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectiveZoom : IZoom
{
    Vector3 normalisedCameraPosition;
    float currentZoomLevel;
    public PerspectiveZoom(Camera cam, Vector3 offset, float startingZoom)
    {
        normalisedCameraPosition = new Vector3(0f, Mathf.Abs(offset.y), -Mathf.Abs(offset.x)).normalized;
        currentZoomLevel = startingZoom;
        PositionCamera(cam);
    }

    private void PositionCamera (Camera cam)
    {
        cam.transform.localPosition = normalisedCameraPosition * currentZoomLevel;
    }

    public void ZoomIn(Camera cam, float delta, float nearZoomLimit)
    {
        if (currentZoomLevel <= nearZoomLimit) return;
        currentZoomLevel = Mathf.Max(currentZoomLevel - delta, nearZoomLimit);
        PositionCamera(cam);
    }

    public void ZoomOut(Camera cam, float delta, float farZoomLimit)
    {
        if (currentZoomLevel >= farZoomLimit) return;
        currentZoomLevel = Mathf.Min(currentZoomLevel - delta, farZoomLimit);
        PositionCamera(cam);
    }

    public void ZoomInMax(Camera cam, float nearZoomLimit)
    {
        currentZoomLevel = nearZoomLimit;
        PositionCamera(cam);
    }

    public void ZoomOutMax(Camera cam, float farZoomLimit)
    {
        currentZoomLevel = farZoomLimit;
        PositionCamera(cam);
    }
}
