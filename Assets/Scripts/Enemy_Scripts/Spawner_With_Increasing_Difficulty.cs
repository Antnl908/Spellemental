using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner_With_Increasing_Difficulty : MonoBehaviour
{
    [SerializeField]
    private List<string> enemyPoolNames = new() { "Skeleton_Pool" };

    [SerializeField]
    private int normalPerWave;

    [SerializeField]
    private int firePerWave;

    [SerializeField]
    private int icePerWave;

    [SerializeField]
    private int lightningPerWave;

    [SerializeField]
    private int earthPerWave;

    [SerializeField]
    private int windPerWave;

    [SerializeField]
    private float spawnDelay = 0.5f;

    [Range(0, 100)]
    [SerializeField]
    private int largeEnemyChance = 5;

    [SerializeField]
    private float largeEnemySizeMultiplier = 1.1f;

    [SerializeField]
    private int extraHealthPerWave = 10;

    [SerializeField]
    private int extraEnemiesPerWave = 10;

    private static int currentEnemyHorde = 0;

    private static int totalSpawners = 0;

    private const int maximumExtraEnemies = 30;

    private const int enemyHordeCountBeforeDifficultyIncrease = 2;

    [SerializeField]
    private float spawnRadius = 2f;

    [SerializeField]
    private bool usesNavMesh = true;

    [SerializeField]
    private bool countsTowardEnemyCount = true;

    private static int currentWave = 0;

    public static int CurrentWave { get => currentWave; }

    private static int currentEnemyCount = 0;

    private bool hasBegunWave = false;

    private bool hasFinishedWave = false;

    private static readonly object lockObject = new object();

    private void Awake()
    {
        currentWave = 0;

        currentEnemyHorde = 0;

        totalSpawners = 0;

        currentEnemyCount = 0;
    }

    private void Start()
    {
        if(countsTowardEnemyCount)
        {
            totalSpawners++;
        }       
    }

    // Update is called once per frame
    void Update()
    {
        if ((currentEnemyHorde + 1) >= enemyHordeCountBeforeDifficultyIncrease * totalSpawners)
        {
            if(currentEnemyCount <= 0)
            {
                currentWave = Mathf.Clamp(currentWave + 1, 0, int.MaxValue);

                currentEnemyHorde = 0;
            }            
        }

        if (!hasBegunWave)
        {
            hasBegunWave = true;

            hasFinishedWave = false;

            StartCoroutine(SpawnWave());
        }
        else
        {
            if (hasFinishedWave && currentEnemyCount <= 0)
            {
                hasBegunWave = false;
            }
        }
    }

    private IEnumerator SpawnWave()
    {
        int currentNormalPerWave = CurrentSpawnAmount(normalPerWave);
        int currentFirePerWave = CurrentSpawnAmount(firePerWave);
        int currentIcePerWave = CurrentSpawnAmount(icePerWave);
        int currentLightningPerWave = CurrentSpawnAmount(lightningPerWave);
        int currentEarthPerWave = CurrentSpawnAmount(earthPerWave);
        int currentWindPerWave = CurrentSpawnAmount(windPerWave);

        foreach (string name in enemyPoolNames)
        {
            for(int i = 0; i < currentNormalPerWave; i++)
            {
                SpawnEnemy(name, Spell.SpellType.None, Spell.SpellType.None);

                yield return new WaitForSeconds(spawnDelay);
            }

            for (int i = 0; i < currentFirePerWave; i++)
            {
                SpawnEnemy(name, Spell.SpellType.Ice, Spell.SpellType.Fire);

                yield return new WaitForSeconds(spawnDelay);
            }

            for (int i = 0; i < currentIcePerWave; i++)
            {
                SpawnEnemy(name, Spell.SpellType.Fire, Spell.SpellType.Ice);

                yield return new WaitForSeconds(spawnDelay);
            }

            for (int i = 0; i < currentLightningPerWave; i++)
            {
                SpawnEnemy(name, Spell.SpellType.Earth, Spell.SpellType.Lightning);

                yield return new WaitForSeconds(spawnDelay);
            }

            for (int i = 0; i < currentEarthPerWave; i++)
            {
                SpawnEnemy(name, Spell.SpellType.Wind, Spell.SpellType.Earth);

                yield return new WaitForSeconds(spawnDelay);
            }

            for (int i = 0; i < currentWindPerWave; i++)
            {
                SpawnEnemy(name, Spell.SpellType.Lightning, Spell.SpellType.Wind);

                yield return new WaitForSeconds(spawnDelay);
            }

            yield return new WaitForSeconds(spawnDelay);
        }

        hasFinishedWave = true;

        if(countsTowardEnemyCount)
        {
            currentEnemyHorde = Mathf.Clamp(currentEnemyHorde + 1, 0, int.MaxValue);
        }        
    }

    private int CurrentSpawnAmount(int baseSpawnAmount)
    {
        if(baseSpawnAmount <= 0)
        {
            return 0;
        }

        int returnedAmount = Mathf.Clamp(baseSpawnAmount + extraEnemiesPerWave * currentWave, 0, maximumExtraEnemies);

        return returnedAmount;
    }

    private void SpawnEnemy(string poolName, Spell.SpellType weakness, Spell.SpellType resistance)
    {
        if(usesNavMesh && Spawner.PointCheck(transform.position, spawnRadius, out Vector3 point))
        {
            GetEnemy(point, poolName, weakness, resistance);
        }
        else if (!usesNavMesh)
        {
            GetEnemy(transform.position, poolName, weakness, resistance);
        }
    }

    private void GetEnemy(Vector3 spawnPoint, string poolName, Spell.SpellType weakness, Spell.SpellType resistance)
    {
        Enemy_Health enemy = (Enemy_Health)Object_Pooler.Pools[poolName].Get();

        enemy.Initialize(spawnPoint, Quaternion.identity, Vector3.zero, Object_Pooler.Pools[poolName]);

        enemy.SetWeaknessAndResistance(weakness, resistance);

        bool isLarge = false;

        if (largeEnemyChance >= Random.Range(1, 101))
        {
            isLarge = true;
        }

        enemy.SetSizeAndSpawner(isLarge, largeEnemySizeMultiplier, currentWave * extraHealthPerWave, this);

        if (countsTowardEnemyCount)
        {
            currentEnemyCount++;
        }
    }

    public void MinusOneEnemy()
    {
        if (countsTowardEnemyCount)
        {
            currentEnemyCount--;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
