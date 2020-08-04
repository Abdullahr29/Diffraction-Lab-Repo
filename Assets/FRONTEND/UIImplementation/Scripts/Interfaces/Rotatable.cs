using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


[RequireComponent(typeof(Rigidbody))]
public class Rotatable : MonoBehaviour, IMovable
{
    //An example script to be attached to an optical component to show how to implement IMovable

    string _id;

    public string ID { get => _id; }
    public Vector3 pos { get => this.transform.position; set => this.transform.position = value; }
    public Quaternion rot { get => this.transform.rotation; set => this.transform.rotation = value; }
    public Rigidbody rigidBody { get => this.GetComponent<Rigidbody>(); }

    private void Start()
    {
        _id = gameObject.name;
        rigidBody.isKinematic = true;
        rigidBody.useGravity = false;
    }   
}