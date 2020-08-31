using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrhtographicZoom : IZoom
{
    public OrhtographicZoom(Camera cam, float startingZoom)
    {
        cam.orthographicSize = startingZoom;
    }

    public void ZoomIn(Camera cam, float delta, float nearZoomLimit)
    {
        if (cam.orthographicSize == nearZoomLimit) return;
        cam.orthographicSize = Mathf.Max(cam.orthographicSize - delta, nearZoomLimit);
    }

    public void ZoomOut(Camera cam, float delta, float farZoomLimit)
    {
        if (cam.orthographicSize == farZoomLimit) return;
        cam.orthographicSize = Mathf.Min(cam.orthographicSize - delta, farZoomLimit);
    }

    public void ZoomInMax(Camera cam, float nearZoomLimit)
    {
        cam.orthographicSize = nearZoomLimit;
    }

    public void ZoomOutMax(Camera cam, float farZoomLimit)
    {
        cam.orthographicSize = farZoomLimit;
    }
}
