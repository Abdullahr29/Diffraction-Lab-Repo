using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//[ExecuteInEditMode]
public class BeamGeneratorScript : MonoBehaviour
{

    [Header("Internal References")]
    public PropagationSystem propagation;


    private List<DiscreteBeam> _descreteBeams; // = new List<DiscreteBeam>();
    private int resolution = 10;

    public GameObject laser;
    public double intensity;
    public Guid Id;

    void OnEnable()
    {
        propagation = ObjectManager.Instance.PropagationManager.GetComponent<PropagationSystem>();
        _descreteBeams = new List<DiscreteBeam>();
        Id = Guid.NewGuid();
        propagation.AddLaser(laser);
        DescreateBeamCreator();
        propagation.AddNewBeams(_descreteBeams);
    }

    void OnDisable()
    {
        DeleteAllDiscreteBeams();
    }

    private void DescreateBeamCreator()
    {
        var radius = laser.transform.Find("BeamOrientationController").transform.localScale.x * 0.5f;
        float delta = Convert.ToSingle(radius / (resolution + 0.5));

        var positions = new List<Vector3>();
        var endingPositions = new List<bool>();

        

        for (int i = -resolution; i <= resolution; i++)
        {
            var first = true;
            for (int j = -resolution; j <= resolution; j++)
            {
                if ((i * i + j * j) * delta * delta < radius * radius)
                {
                    positions.Add(new Vector3(delta * i, 0, delta * j));
                    if (i*i == resolution*resolution | j*j == resolution * resolution)
                    {
                        endingPositions.Add(true);
                    }
                    else
                    {
                        endingPositions.Add(first);
                    }
                    first = false;
                }
                else if (j > 0)
                {
                    endingPositions[endingPositions.Count - 1] = true;
                    break;
                }
            }
        }
        ;

        Debug.Log("Created beams are : " + positions.Count);

        for (int i = 0; i < positions.Count; i++)
        {
            ;
            _descreteBeams.Add(new DiscreteBeam(Id, ParentType.Laser , laser.transform.Find("BeamOrientationController").transform.TransformPoint( positions[i]), laser.transform.Find("BeamOrientationController").transform.TransformDirection(-Vector3.up), endingPositions[i], intensity, 10, 0));
        }

    }


    void OnApplicationQuit()
    {
        //DeleteAllDiscreteBeams();
    }

    private void DeleteAllDiscreteBeams()
    {
        List<Transform> beams = new List<Transform>();


        foreach (Transform child in transform)
        {
            if (child.name == "DescreteBeamPrefab(Clone)")
            {
                beams.Add(child);
            }
        }

        Debug.Log("---------------------------------");
        Debug.Log("CHILD BEAMS FOUND: " + beams.Count);

        foreach ( Transform beam in beams)
        {
            Destroy(beam.gameObject);
        }
    }

    
}


