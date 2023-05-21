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
    private int largeEnemySizeMultiplier = 3;

    [SerializeField]
    private int extraHealthPerWave = 10;

    [SerializeField]
    private float spawnRadius = 2f;

    [SerializeField]
    private int enemyCountBeforeNextWave = 10;

    [SerializeField]
    private bool usesNavMesh = true;

    private int currentWave = 0;

    private int currentEnemyCount = 0;

    private bool hasBegunWave = false;

    private bool hasFinishedWave = false;

    // Update is called once per frame
    void Update()
    {
        if(!hasBegunWave)
        {
            hasBegunWave = true;

            hasFinishedWave = false;

            StartCoroutine(SpawnWave());
        }
        else
        {
            if(hasFinishedWave && currentEnemyCount <= enemyCountBeforeNextWave)
            {
                hasBegunWave = false;
            }
        }
    }

    private IEnumerator SpawnWave()
    {
        foreach(string name in enemyPoolNames)
        {
            for(int i = 0; i < normalPerWave; i++)
            {
                SpawnEnemy(name, Spell.SpellType.None, Spell.SpellType.None);

                yield return new WaitForSeconds(spawnDelay);
            }

            for (int i = 0; i < firePerWave; i++)
            {
                SpawnEnemy(name, Spell.SpellType.Ice, Spell.SpellType.Fire);

                yield return new WaitForSeconds(spawnDelay);
            }

            for (int i = 0; i < icePerWave; i++)
            {
                SpawnEnemy(name, Spell.SpellType.Fire, Spell.SpellType.Ice);

                yield return new WaitForSeconds(spawnDelay);
            }

            for (int i = 0; i < lightningPerWave; i++)
            {
                SpawnEnemy(name, Spell.SpellType.Earth, Spell.SpellType.Lightning);

                yield return new WaitForSeconds(spawnDelay);
            }

            for (int i = 0; i < earthPerWave; i++)
            {
                SpawnEnemy(name, Spell.SpellType.Wind, Spell.SpellType.Earth);

                yield return new WaitForSeconds(spawnDelay);
            }

            for (int i = 0; i < windPerWave; i++)
            {
                SpawnEnemy(name, Spell.SpellType.Lightning, Spell.SpellType.Wind);

                yield return new WaitForSeconds(spawnDelay);
            }

            yield return new WaitForSeconds(spawnDelay);
        }

        hasFinishedWave = true;

        currentWave = Mathf.Clamp(currentWave++, 0, int.MaxValue);
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

        currentEnemyCount++;
    }

    public void MinusOneEnemy()
    {
        currentEnemyCount--;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
