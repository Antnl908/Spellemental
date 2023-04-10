using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spell_Projectile : Pooling_Object
{
    // Made by Daniel.

    private Rigidbody rb;

    private int damage;

    private int effectDamage;

    private int effectBuildUp;

    private Spell.SpellType spellType;

    [SerializeField]
    private Transform point0;

    [SerializeField]
    private Transform point1;

    [SerializeField]
    private float damageRadius = 0.1f;

    [SerializeField]
    private LayerMask enemyLayer;

    [SerializeField]
    private LayerMask terrainLayer;

    [SerializeField]
    private Color capsuleColor = Color.red;

    private IObjectPool<Pooling_Object> pool;

    private float destructionTime;

    [SerializeField]
    private string stationarySpellPoolName = "Error";

    private IObjectPool<Pooling_Object> stationarySpellPool;

    [Range(0, 1000)]
    [SerializeField]
    private float spawnStationaryRangeAboveTransform = 0f;

    [Range(0, 1000)]
    [SerializeField]
    private float spawnStationaryRange = 0f;

    [SerializeField]
    private float spawnOffset;

    private bool gotAHit = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if(stationarySpellPoolName != "Error")
        {
            stationarySpellPool = Object_Pooler.Pools[stationarySpellPoolName];
        }
    }

    public void Initialize(int damage, int effectDamage, int effectBuildUp, Spell.SpellType spellType, 
        Vector3 direction, Vector3 position, Quaternion rotation, IObjectPool<Pooling_Object> pool, float destructionTime)
    {
        rb.velocity = Vector3.zero;

        this.damage = damage;
        this.spellType = spellType;
        this.effectDamage = effectDamage;
        this.effectBuildUp = effectBuildUp;
        this.destructionTime = destructionTime;

        gotAHit = false;

        transform.SetPositionAndRotation(position + (direction.normalized * spawnOffset), rotation);

        rb.AddForce(direction, ForceMode.Impulse);

        this.pool = pool;
    }

    private void Update()
    {
        destructionTime -= Time.deltaTime;

        if(destructionTime <= 0 && enabled)
        {
            pool.Release(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckHits();
    }

    private void CheckHits()
    {
        Collider[] enemyColliders = Physics.OverlapCapsule(point0.position, point1.position, damageRadius, enemyLayer);

        for (int i = 0; i < enemyColliders.Length; i++)
        {
            if (enemyColliders[i].gameObject != gameObject)
            {
                IDamageable damagable = enemyColliders[i].transform.GetComponent<IDamageable>();

                bool gotAKill = false;

                if (damagable != null)
                {
                    gotAKill = (bool)(damagable?.TryToDestroyDamageable(damage, spellType));

                    gotAHit = true;
                }

                if (gotAKill)
                {
                    Player_Health.killCount++;
                }

                IMagicEffect magicEffect = enemyColliders[i].transform.GetComponent<IMagicEffect>();

                magicEffect?.ApplyMagicEffect(effectDamage, effectBuildUp, spellType);
            }
        }

        if (stationarySpellPoolName != "Error")
        {
            if (Physics.Raycast(transform.position + Vector3.up * spawnStationaryRangeAboveTransform, 
                                                            Vector3.down, out RaycastHit hitInfo, spawnStationaryRange, terrainLayer))
            {
                Spell_Stationary stationary = (Spell_Stationary)stationarySpellPool.Get();

                //Rotates it along the ground.
                Quaternion rotation = Quaternion.FromToRotation(transform.up, hitInfo.normal) * transform.rotation;


                stationary.Initialize(hitInfo.point, rotation, stationarySpellPool);
            }
        }
        
        if(enemyColliders.Length <= 0)
        {
            Collider[] terrainColliders = Physics.OverlapCapsule(point0.position, point1.position, damageRadius * 2, terrainLayer);

            if (terrainColliders.Length > 0)
            {
                gotAHit = true;
            }
        }

        if (gotAHit)
        {
            pool.Release(this);
        }       
    }

    public override void Initialize(Vector3 position, Quaternion rotation, Vector3 direction)
    {
        rb.AddForce(direction, ForceMode.Impulse);
    }

    private void OnDrawGizmosSelected()
    {
        Extra_Gizmos.DrawCapsule(point0.position, point1.position, damageRadius, capsuleColor);

        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, 
                           transform.position.y - (spawnStationaryRange - spawnStationaryRangeAboveTransform), transform.position.z));

        Gizmos.DrawLine(transform.position, transform.position + Vector3.forward * -spawnOffset);
        Gizmos.DrawLine(transform.position + Vector3.right * 2, transform.position + Vector3.up * spawnStationaryRangeAboveTransform
                                                                                                                 + Vector3.right * 2);

        Gizmos.color = Color.yellow;
    }
}
