using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LensBehaviour : MonoBehaviour
{
    [Header("INTERNAL REFERENCES")]
    public Transform anchor;

    private Vector3 pos;
    private Quaternion rot;

    public Vector3 Pos { get { return pos; } }
    public Quaternion Rot { get { return rot; } }

    // Start is called before the first frame update
    void Start()
    {
        pos = anchor.position;
        rot = anchor.rotation;
    }
}
