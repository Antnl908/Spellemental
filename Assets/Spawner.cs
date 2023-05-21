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

    int nbrOfNormalEnemiesSpawned;

    int nbrOfFireEnemiesSpawned;

    int nbrOfIceEnemiesSpawned;

    int nbrOfLightningEnemiesSpawned;

    int nbrOfEarthEnemiesSpawned;

    int nbrOfWindEnemiesSpawned;

    float timer;

    private bool initialSpawnIsDone = false;

    [SerializeField]
    private string enemyPoolName = "Skeleton_Pool";

    private const int maxEnemyCount = 100;

    public static int CurrentEnemyCount { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        nbrOfNormalEnemiesSpawned = 0;
        nbrOfFireEnemiesSpawned = 0;
        nbrOfIceEnemiesSpawned = 0;
        nbrOfLightningEnemiesSpawned = 0;
        nbrOfEarthEnemiesSpawned = 0;
        nbrOfWindEnemiesSpawned = 0;

        CurrentEnemyCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log(timer);

        if(CurrentEnemyCount < maxEnemyCount)
        {
            timer += Time.deltaTime;
            if (!initialSpawnIsDone && timer > waveConfig.initialSpawnDelay)
            {
                CreateWave();

                initialSpawnIsDone = true;
            }
            else if (timer > waveConfig.spawnDelay)
            {
                //Instantiate(enemy, new Vector3(transform.position.x + Random.Range(-waveConfig.spawnArea, waveConfig.spawnArea), 0,
                //transform.position.z + Random.Range(-waveConfig.spawnArea, waveConfig.spawnArea)), Quaternion.identity);

                //nbrOfEnemiesSpawned++;
                //timer = 0f;

                CreateWave();
            }
        }      
    }

    private void CreateWave()
    {
        if (nbrOfNormalEnemiesSpawned < waveConfig.nbrOfBasicSkeletons)
        {
            Spawn(Spell.SpellType.None, ref nbrOfNormalEnemiesSpawned);
        }

        if (nbrOfFireEnemiesSpawned < waveConfig.nbrOfFireSkeletons)
        {
            Spawn(Spell.SpellType.Ice, ref nbrOfFireEnemiesSpawned);
        }

        if (nbrOfIceEnemiesSpawned < waveConfig.nbrOfIceSkeletons)
        {
            Spawn(Spell.SpellType.Fire, ref nbrOfIceEnemiesSpawned);
        }

        if (nbrOfLightningEnemiesSpawned < waveConfig.nbrOfLightningSkeletons)
        {
            Spawn(Spell.SpellType.Earth, ref nbrOfLightningEnemiesSpawned);
        }

        if (nbrOfEarthEnemiesSpawned < waveConfig.nbrOfEarthSkeletons)
        {
            Spawn(Spell.SpellType.Wind, ref nbrOfEarthEnemiesSpawned);
        }

        if (nbrOfWindEnemiesSpawned < waveConfig.nbrOfWindSkeletons)
        {
            Spawn(Spell.SpellType.Lightning, ref nbrOfWindEnemiesSpawned);
        }
    }

    void Spawn(Spell.SpellType weakness, ref int nbrOfEnemies)
    {
        if (PointCheck(transform.position, waveConfig.spawnArea, out Vector3 point))
        {
            Enemy_Health enemy = (Enemy_Health)Object_Pooler.Pools[enemyPoolName].Get();

            enemy.Initialize(point, Quaternion.identity, Vector3.zero, Object_Pooler.Pools[enemyPoolName]);

            switch (weakness)
            {
                case Spell.SpellType.Fire:
                    enemy.SetWeaknessAndResistance(Spell.SpellType.Ice, Spell.SpellType.Fire);
                    break;

                    case Spell.SpellType.Ice:
                    enemy.SetWeaknessAndResistance(Spell.SpellType.Fire, Spell.SpellType.Ice);
                    break;

                    case Spell.SpellType.Lightning:
                    enemy.SetWeaknessAndResistance(Spell.SpellType.Earth, Spell.SpellType.Lightning);
                    break;

                    case Spell.SpellType.Earth:
                    enemy.SetWeaknessAndResistance(Spell.SpellType.Wind, Spell.SpellType.Earth);
                    break;

                case Spell.SpellType.Wind:
                    enemy.SetWeaknessAndResistance(Spell.SpellType.Lightning, Spell.SpellType.Wind);
                    break;

                    default:
                    enemy.SetWeaknessAndResistance(Spell.SpellType.None, Spell.SpellType.None);
                    break;
            }

            nbrOfEnemies++;

            CurrentEnemyCount++;

            timer = 0f;
        }
    }

    bool PointCheck(Vector3 center, float radius, out Vector3 spawnPoint)
    {
        for(int i = 0; i < 30; i++)
        {
            Vector3 randomPosition = center + Random.insideUnitSphere * radius;
            if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                spawnPoint = hit.position;
                return true;
            }
        }
        spawnPoint = center;
        return false;
    }
}
