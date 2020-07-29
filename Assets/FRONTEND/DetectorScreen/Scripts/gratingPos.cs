using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gratingPos : MonoBehaviour
{
    Vector3 pos;
    public Vector3 Pos { get { return pos; } }

    private void Start()
    {
        pos = transform.position;
    }
}
