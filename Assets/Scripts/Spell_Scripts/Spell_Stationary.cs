using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class Spell_Stationary : Pooling_Object
{
    //Made by Daniel.

    private int damage;

    private int effectDamage;

    private int effectBuildUp;

    private Spell.SpellType spellType;

    [SerializeField]
    private bool destroyedAfterSomeTime = false;

    private IObjectPool<Pooling_Object> pool;

    public IObjectPool<Pooling_Object> Pool { get => pool; }

    private float destructionTime = 1f;

    private float currentDestructionTime;

    [SerializeField]
    private float height;

    [SerializeField]
    private float width;

    [SerializeField]
    private float depth;

    [SerializeField]
    private float heightOffset = 0.2f;

    [SerializeField]
    private LayerMask enemyLayer;

    [SerializeField]
    private bool destroyOnHit = false;

    private bool gotAHit;

    private const float hitDelayAtStart = 0.1f;

    private float hitDelay;

    [SerializeField]
    private bool usesRotation = true;

    [SerializeField]
    private Vector3 hitBoxOffset = new(0, 0, 0);

    public event EventHandler OnInitialisation;

    [SerializeField]
    private string visualEffectPoolName = "Error";

#nullable enable
    [SerializeField]
    private Transform? vfxSpawnPos;
#nullable disable

    private float effectTimer;
    private const float effectDelay = 0.1f;

    #region OverlapSphere
    [Header("OverlapSphere")]
    [SerializeField] float range;
    int count;
    readonly Collider[] colliders = new Collider[10];
    [SerializeField] private bool useOverlapSphere = false;
    private float checkTimer;
    [SerializeField] private float checkTime = 0.25f;
    #endregion

    // Update is called once per frame
    void Update()
    {
        if (destroyedAfterSomeTime && enabled)
        {
            currentDestructionTime -= Time.deltaTime;
            if (useOverlapSphere) { OverlapHit(); }
            if (currentDestructionTime <= 0)
            {
                pool.Release(this);
            }
        }

        if(hitDelay > 0)
        {
            hitDelay -= Time.deltaTime;
        }
    }

    public void Initialize(Vector3 position, Quaternion rotation, IObjectPool<Pooling_Object> pool, 
                 int damage, int effectDamage, int effectBuildUp, Spell.SpellType spellType, float destructionTime)
    {
        this.damage = damage;
        this.effectDamage = effectDamage;
        this.effectBuildUp = effectBuildUp;
        this.spellType = spellType;
        this.destructionTime = destructionTime;

        gotAHit = false;

        hitDelay = hitDelayAtStart;

        Initialize(position, rotation, pool);

        OnInitialisation?.Invoke(this, EventArgs.Empty);

        if(OnInitialisation == null)
        {
            Debug.Log("Initialsation is null");
        }
    }

    public void Initialize(Vector3 position, Quaternion rotation, IObjectPool<Pooling_Object> pool)
    {
        //Has to be done in this order for it to work.
#pragma warning disable UNT0022 // Inefficient position/rotation assignment
        if(usesRotation)
        {
            transform.rotation = rotation;
        }


        transform.position = position + transform.up * heightOffset;
#pragma warning restore UNT0022 // Inefficient position/rotation assignment

        this.pool = pool;

        currentDestructionTime = destructionTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (useOverlapSphere)
        {
            InstantCheckHits(other);
        }
        else if(destroyOnHit)
        {
            CheckHits();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!useOverlapSphere && !destroyOnHit)
        {
            CheckHits();
        }
    }

    public void CheckHits()
    {
        if (!isActiveAndEnabled)
        {
            return;
        }

        //Detta kan förbättras senare.
        if (!destroyOnHit)
        {
            effectTimer -= Time.deltaTime;
            if (effectTimer > 0.0f) { return; }
            effectTimer = effectDelay;
        }

        Collider[] colliders = Physics.OverlapBox(transform.position + hitBoxOffset, new Vector3(width / 2, height / 2, depth / 2), transform.rotation, enemyLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject && hitDelay <= 0)
            {
                IMagicEffect magicEffect = colliders[i].transform.GetComponent<IMagicEffect>();

                magicEffect?.ApplyMagicEffect(effectDamage, effectBuildUp, spellType);

                IDamageable damagable = colliders[i].transform.GetComponent<IDamageable>();

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

                gotAHit = true;
            }
        }

        if(gotAHit && destroyOnHit)
        {
            if (visualEffectPoolName != "Error")
            {
                Pooled_VFX vfx = (Pooled_VFX)Object_Pooler.Pools[visualEffectPoolName].Get();

                vfx.Initialize(vfxSpawnPos.position, transform.rotation, Vector3.zero, Object_Pooler.Pools[visualEffectPoolName]);
            }

            pool.Release(this);
        }
    }

    public void SetGotHitToTrue()
    {
        gotAHit = true;
    }

    private void InstantCheckHits(Collider other)
    {
        IMagicEffect magicEffect = other.transform.GetComponent<IMagicEffect>();

        magicEffect?.ApplyMagicEffect(effectDamage, effectBuildUp, spellType);

        IDamageable damagable = other.transform.GetComponent<IDamageable>();

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

        //if (effectObjectPoolName != "Error")
        //{
        //    for (int x = 0; x < effectInstanceAmount; x++)
        //    {
        //        Pooling_Object pooling_Object = Object_Pooler.Pools[effectObjectPoolName].Get();

        //        pooling_Object.Initialize(transform.position, Quaternion.identity,
        //                                     other.transform.position, Object_Pooler.Pools[effectObjectPoolName]);
        //    }
        //}

        //if (stationarySpellPoolName != "Error")
        //{
        //    if (Physics.Raycast(transform.position + Vector3.up * spawnStationaryRangeAboveTransform,
        //                                                    Vector3.down, out RaycastHit hitInfo, spawnStationaryRange, terrainLayer))
        //    {
        //        Spell_Stationary stationary = (Spell_Stationary)stationarySpellPool.Get();

        //        //Rotates it along the ground.
        //        Quaternion rotation = Quaternion.FromToRotation(transform.up, hitInfo.normal) * transform.rotation;


        //        stationary.Initialize(hitInfo.point, rotation, stationarySpellPool);
        //    }
        //}

        //if (other.gameObject.layer == terrainLayer)
        //{
        //    gotAHit = true;
        //}

        if (gotAHit && destroyOnHit)
        {
            pool.Release(this);
        }
    }

    private void OverlapHit()
    {
        //potential lag fix

        checkTimer -= Time.deltaTime;
        if (checkTimer > 0.0f) { return; }
        checkTimer = checkTime;

        count = Physics.OverlapSphereNonAlloc(transform.position, range, colliders, enemyLayer, QueryTriggerInteraction.Collide);
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    IMagicEffect magicEffect = colliders[i].transform.GetComponent<IMagicEffect>();

                    magicEffect?.ApplyMagicEffect(effectDamage, effectBuildUp, spellType);

                    IDamageable damagable = colliders[i].transform.GetComponent<IDamageable>();

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

                    //if (effectObjectPoolName != "Error")
                    //{
                    //    for (int x = 0; x < effectInstanceAmount; x++)
                    //    {
                    //        Pooling_Object pooling_Object = Object_Pooler.Pools[effectObjectPoolName].Get();

                    //        pooling_Object.Initialize(transform.position, Quaternion.identity,
                    //                                     colliders[i].transform.position, Object_Pooler.Pools[effectObjectPoolName]);
                    //    }
                    //}
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position + hitBoxOffset, new Vector3(width / 2, height / 2, depth / 2));

        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y + heightOffset, 
                                                                                                      transform.position.z));

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
