using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Spawner : MonoBehaviour
{
    [SerializeField]
    private Enemy_Health enemy;

    [SerializeField]
    private Spell.SpellType weakness;

    [SerializeField]
    private Spell.SpellType resistance;

    private const string poolName = "Skeleton_Pool";

    // Start is called before the first frame update
    void Start()
    {
        enemy.Initialize(transform.position, Quaternion.identity, Vector3.zero, Object_Pooler.Pools[poolName]);

        enemy.SetWeaknessAndResistance(weakness, resistance);

        enemy.SetSizeAndSpawner(false, 1, 0, null);
    }
}
