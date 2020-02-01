using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardSpawnPoint : MonoBehaviour
{
    [Serializable]
    public enum HazardType
    {
        Fire,
        Lightning,
        YellowCloud
    }

    public HazardType hazardType = HazardType.Fire;
    public GameObject fireHazard;
    public GameObject lightningHazard;
    public GameObject cloudHazard;

    public bool HasHazard 
    { 
        get
        {
            return transform.childCount > 0;
        } 
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnHazard()
    {
        switch(hazardType)
        {
            case HazardType.Fire:
                GameObject.Instantiate(fireHazard, transform);
                break;
            case HazardType.Lightning:
                GameObject.Instantiate(lightningHazard, transform);
                break;
            case HazardType.YellowCloud:
                GameObject.Instantiate(cloudHazard, transform);
                break;
        }
    }

    void OnDrawGizmos()
    {
        switch(hazardType)
        {
            case HazardType.Fire:
                Gizmos.DrawIcon(transform.position, "fireplace.png");
                break;
            case HazardType.Lightning:
                Gizmos.DrawIcon(transform.position, "power.png");
                break;
            case HazardType.YellowCloud:
                Gizmos.DrawIcon(transform.position, "cloud.png");
                break;
        }
    }
}
