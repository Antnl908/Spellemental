using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

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
    private float spawnStationaryRange = 0f;

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
        Collider[] colliders = Physics.OverlapCapsule(point0.position, point1.position, damageRadius, enemyLayer);

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
            }
        }

        if (stationarySpellPoolName != "Error")
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, spawnStationaryRange))
            {
                Spell_Stationary stationary = (Spell_Stationary)stationarySpellPool.Get();

                Quaternion rotation = new();

                if(Physics.Raycast(transform.position + Vector3.right, Vector3.down, out RaycastHit rightHitInfo,
                                                                  spawnStationaryRange) && rightHitInfo.point.y != hitInfo.point.y)
                {
                    rotation = Quaternion.LookRotation(hitInfo.point - rightHitInfo.point);
                }
                else if (Physics.Raycast(transform.position + Vector3.left, Vector3.down, out RaycastHit leftHitInfo,
                                                                  spawnStationaryRange) && leftHitInfo.point.y != hitInfo.point.y)
                {
                    rotation = Quaternion.LookRotation(hitInfo.point - leftHitInfo.point);
                }
                else if (Physics.Raycast(transform.position + Vector3.forward, Vector3.down, out RaycastHit forwardHitInfo,
                                                                  spawnStationaryRange) && forwardHitInfo.point.y != hitInfo.point.y)
                {
                    rotation = Quaternion.LookRotation(hitInfo.point - forwardHitInfo.point);
                }
                else if (Physics.Raycast(transform.position + Vector3.back, Vector3.down, out RaycastHit backwardHitInfo,
                                                                  spawnStationaryRange) && backwardHitInfo.point.y != hitInfo.point.y)
                {
                    rotation = Quaternion.LookRotation(hitInfo.point - backwardHitInfo.point);
                }

                stationary.Initialize(hitInfo.point, rotation, stationarySpellPool);
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

        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - spawnStationaryRange,
                                                                                                        transform.position.z));
    }
}
