using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor;
using System;


[System.Serializable]
public class Mesh
{
    public obj item;
    public GameObject prefab;

    public Mesh(obj item, GameObject prefab)
    {
        this.item = item;
        this.prefab = prefab;
    }
}

public class MeshesInventory : MonoBehaviour
{
    // Turn meshes manager into singleton
    private static MeshesInventory _instance;
    public static MeshesInventory Instance
    {   
        get
        {
            if (_instance == null)
            {
                Debug.LogError("MeshesInventory is NULL.");
            }                

            return _instance;
        }
    }

    [SerializeField]
    public Transform _labMeshes;


    [SerializeField]
    private List<Mesh> availableMeshes = new List<Mesh>();
    private Dictionary<obj, GameObject> availableMeshesDict = new Dictionary<obj, GameObject>();

    private Dictionary<obj, GameObject> activeMeshes = new Dictionary<obj, GameObject>();



    private void Awake()
    {
        _instance = this;
        //Reupdate our records of active objects to ensure accuracy
        DictionaryMeshes();
        foreach (obj id in Enum.GetValues(typeof(obj)))
        {
            if (!(id == obj.camMain | id == obj.camScreen | id == obj.measure | id == obj.movement | id == obj.rotation))
            {                
                SearchForActive(id);
            }           
        }        
    }



    private void DictionaryMeshes()
    {
        foreach(Mesh mesh in availableMeshes)
        {
            availableMeshesDict.Add(mesh.item, mesh.prefab);
        }
    }
    public GameObject returnPrefabFromKey(obj item)
    {
        if (availableMeshesDict.ContainsKey(item) == false)
        {
            DictionaryMeshes();
            return availableMeshesDict[item];
        }

        return availableMeshesDict[item];
    }
    public GameObject ReturnActiveMeshFromKey(obj item)
    {
        if (activeMeshes.ContainsKey(item) != false)
        {
            return availableMeshesDict[item];
        }
        else
        {
            return null;
        }        
    }

    public void InstantiateItem(obj item)
    {
        if (activeMeshes.ContainsKey(item) == false)
        {
            GameObject prefab = returnPrefabFromKey(item);
            GameObject newMesh = Instantiate(prefab);
            newMesh.name = prefab.name;
            newMesh.transform.SetParent(_labMeshes, false);
            activeMeshes.Add(item, newMesh);
            newMesh.layer = 8;
            ObjectManager.Instance.AddRef(item, newMesh);
            ObjectManager.Instance.CheckRequiredComponents();
            //instantiatedMeshes.Add(item);
        }
    }

    public void InstantiateTwo(obj mesh, obj manager)
    {
        //Need manager instantiated before dependent objects
        if (activeMeshes.ContainsKey(manager) == false)
        {
            GameObject managerPrefab = returnPrefabFromKey(manager);
            GameObject newManager = Instantiate(managerPrefab);
            newManager.name = managerPrefab.name;
            activeMeshes.Add(manager, newManager);
            ObjectManager.Instance.AddRef(manager, newManager);

        }

        InstantiateItem(mesh);
    }

    public void SearchForActive(obj item)
    {
        GameObject targetObject;
        targetObject = GameObject.Find(availableMeshesDict[item].name);

        //check whether prefab has been instantiated int the scene
        if (targetObject != null)
        {
            activeMeshes[item] = targetObject;
        }
        else if(activeMeshes.ContainsKey(item))
        {
            //Remove old reference if present
            activeMeshes.Remove(item);
        }           
    }

}