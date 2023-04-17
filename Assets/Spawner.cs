using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

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
            Instantiate(enemy, new Vector3(transform.position.x + Random.Range(-waveConfig.spawnArea, waveConfig.spawnArea), 0,
            transform.position.z + Random.Range(-waveConfig.spawnArea, waveConfig.spawnArea)), Quaternion.identity);

            nbrOfEnemiesSpawned++;
            timer = 0f;
        }
    }
}
