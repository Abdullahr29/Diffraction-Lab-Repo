using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IZoom
{
    void ZoomIn(Camera cam, float delta, float nearZoomLimit);
    void ZoomOut(Camera cam, float delta, float farZoomLimit);

    void ZoomInMax(Camera cam, float nearZoomLimit);
    void ZoomOutMax(Camera cam, float farZoomLimit);
}
