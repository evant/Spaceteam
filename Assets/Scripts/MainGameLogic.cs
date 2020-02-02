using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

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
            var players = FindObjectsOfType<Spaceguy>();

            var spawns = FindObjectsOfType<HazardSpawnPoint>();
            var index = Random.Range(0, spawns.Length);
            for (int i = index; i < spawns.Length; ++i)
            {
                var spawnPoint = spawns[index];
                if (!spawnPoint.HasHazard)
                {
                    var skilledPlayer = players.FirstOrDefault((p) =>
                    {
                        return spawnPoint.hazardType == p.playerAbility;
                    });
                    if(skilledPlayer != null)
                    {
                        spawnPoint.SpawnHazard();
                    }
                    break;
                }
            }
            _timeToNextHazard = hazardSpawnTime;
        }
    }
}
