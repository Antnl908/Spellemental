using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private LayerMask enemyAndGroundLayer;

    [SerializeField]
    private Color capsuleColor = Color.red;

    private IObjectPool<Pooling_Object> pool;

    private float destructionTime;

    [SerializeField]
    private string stationarySpellPoolName = "Error";

    private IObjectPool<Pooling_Object> stationarySpellPool;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if(stationarySpellPoolName != "Error")
        {
            stationarySpellPool = Object_Pooler.Pools[stationarySpellPoolName];

            Debug.Log("Added to spell the pool " + stationarySpellPoolName);
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

        transform.SetPositionAndRotation(position, rotation);

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
        Collider[] colliders = Physics.OverlapCapsule(point0.position, point1.position, damageRadius, enemyAndGroundLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                IDamageable damagable = colliders[i].transform.GetComponent<IDamageable>();

                bool gotAKill = false;

                if (damagable != null)
                {
                    gotAKill = (bool)(damagable?.TryToDestroyDamageable(damage, spellType));
                }

                if (gotAKill)
                {
                    Player_Health.killCount++;
                }

                IMagicEffect magicEffect = colliders[i].transform.GetComponent<IMagicEffect>();

                magicEffect?.ApplyMagicEffect(effectDamage, effectBuildUp, spellType);

                if(stationarySpellPoolName != "Error")
                {
                    Spell_Stationary stationary = (Spell_Stationary)stationarySpellPool.Get();

                    stationary.Initialize(colliders[i].ClosestPoint(transform.position), Quaternion.identity, stationarySpellPool);

                    Debug.Log("Spawned stationary");
                }
            }
        }

        pool.Release(this);
    }

    public override void Initialize(Vector3 position, Quaternion rotation, Vector3 direction)
    {
        rb.AddForce(direction, ForceMode.Impulse);
    }

    private void OnDrawGizmosSelected()
    {
        Extra_Gizmos.DrawCapsule(point0.position, point1.position, damageRadius, capsuleColor);
    }
}
