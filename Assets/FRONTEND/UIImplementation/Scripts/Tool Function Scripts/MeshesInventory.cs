using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor;

[System.Serializable]
public enum MeshID
{
    board,
    laser,
    propagationSystem,
    lens,
    grating,
    screen,
    cmosCamera,
    emailManager
}

[System.Serializable]
public class Mesh
{
    public MeshID item;
    public GameObject prefab;

    public Mesh(MeshID item, GameObject prefab)
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
    {   get
    {
            if (_instance == null)
                Debug.LogError("MeshesInventory is NULL.");

            return _instance;

        }
    }
    
    private void Awake()
    {
        _instance = this; 
    }

    [SerializeField]
    private Transform _labMeshes;

    [SerializeField]
    private List<Mesh> availableMeshes = new List<Mesh>();
    private List<MeshID> instantiatedMeshes = new List<MeshID>();

    public Dictionary<MeshID, GameObject> availableMeshesDict = new Dictionary<MeshID, GameObject>();

    private GameObject board, laser, lens, slit, grating, screen, cmosCamera;

    private void DictionaryMeshes()
    {
        foreach(Mesh mesh in availableMeshes)
        {
            availableMeshesDict.Add(mesh.item, mesh.prefab);
        }
    }
    private GameObject returnPrefabFromKey(MeshID item)
    {
        if (availableMeshesDict.ContainsKey(item) == false)
        {
            DictionaryMeshes();
            return availableMeshesDict[item];
        }

        return availableMeshesDict[item];
    }

    public void InstantiateItem(MeshID item)
    {
        if (instantiatedMeshes.Contains(item) == false)
        {
            GameObject prefab = returnPrefabFromKey(item);
            GameObject newMesh = Instantiate(prefab);
            newMesh.name = prefab.name;
            newMesh.transform.SetParent(_labMeshes, false);
            instantiatedMeshes.Add(item);
        }
    }

    public void InstantiateTwo(MeshID mesh, MeshID manager)
    {
        InstantiateItem(mesh);

        if (instantiatedMeshes.Contains(manager) == false)
        {
            GameObject managerPrefab = returnPrefabFromKey(manager);
            GameObject newManager = Instantiate(managerPrefab);
            newManager.name = managerPrefab.name;
            instantiatedMeshes.Add(manager);
        }

    }

}