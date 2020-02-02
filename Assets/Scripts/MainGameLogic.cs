using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameLogic : MonoBehaviour
{
    public float hazardSpawnTime = 8.0f;

    private float _timeToNextHazard;

    void Start()
    {
        _timeToNextHazard = hazardSpawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        _timeToNextHazard -= Time.deltaTime;
        if (_timeToNextHazard < 0)
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
            _timeToNextHazard = hazardSpawnTime;
        }
    }
}
