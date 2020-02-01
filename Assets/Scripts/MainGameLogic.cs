using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameLogic : MonoBehaviour
{
    public float fireSpawnTime = 8.0f;
    public GameObject firePrefab;

    private float _timeToNextFire;

    void Start()
    {
        _timeToNextFire = fireSpawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (firePrefab != null)
        {
            _timeToNextFire -= Time.deltaTime;
            if (_timeToNextFire < 0)
            {
                var spawns = FindObjectsOfType<HazardSpawnPoint>();
                for (int i = 0; i < 5; ++i)
                {
                    var index = Random.Range(0, spawns.Length);
                    var spawnPoint = spawns[index];
                    if (!spawnPoint.HasHazard)
                    {
                        spawnPoint.SpawnHazard();
                        break;
                    }
                }
                _timeToNextFire = fireSpawnTime;
            }
        }
    }
}
