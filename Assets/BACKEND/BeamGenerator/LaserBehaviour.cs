using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehaviour : MonoBehaviour
{
    [Header("INTERNAL REFERENCES")]
    public PropagationSystem Propagation;
    public BeamGeneratorScript BeamGenerator;

    private GameObject newLaser;
    public GameObject newProp;
    public GameObject NewLaser { get { return newLaser; } }

    // for enabling and disabling the propagation system.
    private bool active = true;

    // use this when you want to activate / deactivate from script
    public bool Active
    {
        get { return active; }
        set { active = value;}
    }

    private void Awake()
    {
        active = false;
        Propagation = ObjectManager.Instance.PropagationManager.GetComponent<PropagationSystem>();
        BeamGenerator.gameObject.SetActive(false);
        Propagation.gameObject.SetActive(false);
        Debug.Log("LASER INITIALISED");
    }

    // use this from UI which handles the toggle
    public void ToggleActive()
    {
        Debug.Log("Toggle");
        if (active)
        {
            DeactivateLaser();
        }
        else
        {
            ActivateLaser();
            
        }
    }

    public void ActivateLaser()
    {
        active = true;
        BeamGenerator.gameObject.SetActive(true);
        Propagation = ObjectManager.Instance.PropagationManager.GetComponent<PropagationSystem>();
        Propagation.gameObject.SetActive(true);
        Debug.Log("LASER ACTIVATED");
    }

    public void DeactivateLaser()
    {
        active = false;

        newProp = Instantiate(Propagation.gameObject);
        newProp.name = Propagation.gameObject.name;
        ObjectManager.Instance.PropagationManager = newProp;
        Destroy(Propagation.gameObject);
        Propagation = newProp.GetComponent<PropagationSystem>();
       
        newLaser = Instantiate(this.gameObject, this.transform.parent, true);        
        newLaser.name = this.name;
        ObjectManager.Instance.Laser = newLaser;

        Destroy(this.gameObject);           

        Debug.Log("LASER DEACTIVATED");
    }

    //public void DeactivateLaser()
    //{
    //    active = false;


    //    BeamGenerator.gameObject.SetActive(false);
    //    Propagation.gameObject.SetActive(false);

    //    Debug.Log("LASER DEACTIVATED");
    //}
}
