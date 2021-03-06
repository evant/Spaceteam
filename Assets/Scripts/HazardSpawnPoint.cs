﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HazardSpawnPoint : MonoBehaviour
{
    [Serializable]
    public enum HazardType
    {
        Fire,
        Lightning,
        YellowCloud,
        Alien
    }

    public HazardType hazardType = HazardType.Fire;
    
    public GameObject fireHazard;
    public GameObject lightningHazard;
    public GameObject cloudHazard;
    public GameObject alienHazard;
    public GameObject alienSpawnEffect;

    public bool HasHazard 
    { 
        get
        {
            return transform.childCount > 0;
        } 
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public async void SpawnHazard()
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
            case HazardType.Alien:
                var effect = GameObject.Instantiate(alienSpawnEffect, transform);
                await Task.Delay(250);
                GameObject.Instantiate(alienHazard, transform.position, Quaternion.identity);
                await Task.Delay(500);
                Destroy(effect);
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
            case HazardType.Alien:
                Gizmos.DrawIcon(transform.position, "alien.png");
                break;
        }
    }
}
