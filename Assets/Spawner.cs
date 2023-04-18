using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    [SerializeField] private WaveConfig waveConfig;

    [SerializeField] private GameObject enemy;

    int nbrOfEnemiesSpawned;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        nbrOfEnemiesSpawned = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(timer);

        timer += Time.deltaTime;
        if (timer > waveConfig.spawnDelay && nbrOfEnemiesSpawned < waveConfig.nbrOfBasicSkeletons)
        {
            //Instantiate(enemy, new Vector3(transform.position.x + Random.Range(-waveConfig.spawnArea, waveConfig.spawnArea), 0,
            //transform.position.z + Random.Range(-waveConfig.spawnArea, waveConfig.spawnArea)), Quaternion.identity);

            //nbrOfEnemiesSpawned++;
            //timer = 0f;

            Spawn();
        }
    }

    void Spawn()
    {
        Vector3 point;
        if(PointCheck(transform.position, waveConfig.spawnArea, out point))
        {
            Instantiate(enemy, point, Quaternion.identity);

            nbrOfEnemiesSpawned++;
            timer = 0f;
        }
    }

    bool PointCheck(Vector3 center, float radius, out Vector3 spawnPoint)
    {
        for(int i = 0; i < 30; i++)
        {
            Vector3 randomPosition = center + Random.insideUnitSphere * radius;
            NavMeshHit hit;
            if(NavMesh.SamplePosition(randomPosition, out hit, 1.0f, NavMesh.AllAreas))
            {
                spawnPoint = hit.position;
                return true;
            }
        }
        spawnPoint = center;
        return false;
    }
}
