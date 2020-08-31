using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovable
{
    string ID { get; }
    Vector3 pos { get; set; }
    Quaternion rot { get; set; }
    Rigidbody rigidBody { get;  }

}
