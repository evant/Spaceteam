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
                var spawns = FindObjectsOfType<FireSpawnPoint>();
                for (int i = 0; i < 5; ++i)
                {
                    var index = Random.Range(0, spawns.Length);
                    var spawnPoint = spawns[index];
                    if (spawnPoint.transform.childCount == 0)
                    {
                        Object.Instantiate(firePrefab, spawnPoint.transform);
                        break;
                    }
                }
                _timeToNextFire = fireSpawnTime;
            }
        }
    }
}
