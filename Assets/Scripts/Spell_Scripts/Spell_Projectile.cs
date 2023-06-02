using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.Pool;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;

public class Spell_Projectile : Pooling_Object
{
    // Made by Daniel.

    private Rigidbody rb;

    private int damage;

    public int Damage { get { return damage; } }

    private int effectDamage;

    public int EffectDamage { get { return effectDamage; } }

    private int effectBuildUp;

    public int EffectBuildUp { get { return effectBuildUp; } }

    private Spell.SpellType spellType;

    public Spell.SpellType SpellType { get { return spellType; } }

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

    private readonly Collider[] enemyColliders = new Collider[100];
    private readonly Collider[] terrainColliders = new Collider[100];

    private int hitCount;

    [SerializeField]
    private Color capsuleColor = Color.red;

    private IObjectPool<Pooling_Object> pool;

    public IObjectPool<Pooling_Object> Pool { get { return pool; } }

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

    [SerializeField]
    private bool destroyOnHit = true;

    [SerializeField]
    private bool destroyAfterSomeTime = true;

    [Header("Effects")]
    [SerializeField]
    private string effectObjectPoolName = "Error";

    [SerializeField]
    private int effectInstanceAmount = 3;

    public event EventHandler OnInitialisation;

    [SerializeField]
    private string visualEffectPoolName = "Error";

#nullable enable
    [SerializeField]
    private Transform? vfxSpawnPos;
#nullable disable

    private float effectTimer;

    [SerializeField]
    private float effectDelay = 0.1f;

    #region OverlapSphere
    [Header("OverlapSphere")]
    [SerializeField] float range;
    int count;
    readonly Collider[] colliders = new Collider[10];
    [SerializeField] private bool useOverlapSphere = false;
    private float checkTimer;
    [SerializeField] private float checkTime = 0.25f;

# nullable enable
    [SerializeField]
    private Transform spherePosition;
# nullable disable
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if(stationarySpellPoolName != "Error")
        {
            stationarySpellPool = Object_Pooler.Pools[stationarySpellPoolName];
        }
    }

    /// <summary>
    /// Initializes the spells starting values and shoots it forwards.
    /// </summary>
    /// <param name="damage">How much damage it deals</param>
    /// <param name="effectDamage">How much effect damage it deals</param>
    /// <param name="effectBuildUp">How many percentages of effect build up the spell will give</param>
    /// <param name="spellType">The spells spell type</param>
    /// <param name="direction">The direction the spell is shot in</param>
    /// <param name="position">The position the spell is spawned at</param>
    /// <param name="rotation">The rotation the spell is given</param>
    /// <param name="pool">The pool the spell came from</param>
    /// <param name="destructionTime">How ling it is until the spell is returned to its pool</param>
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

        OnInitialisation?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        if (destroyAfterSomeTime)
        {
            destructionTime -= Time.deltaTime;
            if (useOverlapSphere) { OverlapHit(); }
            if (destructionTime <= 0 && enabled)
            {
                pool.Release(this);
            }
        }       
    }

    private void OnTriggerEnter(Collider other)
    {
        if(useOverlapSphere)
        {
            InstantCheckHits(other);
        }       
    }

    private void OnTriggerStay(Collider other)
    {
        if(!useOverlapSphere)
        {
            CheckHits();
        }
    }

    /// <summary>
    /// Deals damage to any object that implements the IDamageable or the IMagicEffect interfaces. 
    /// Spawns a vfx if it destroys upon hitting something.
    /// If it spawns a stationary spell upon hitting something, then that stationary spell is spawned.
    /// </summary>
    private void CheckHits()
    {
        if (effectObjectPoolName != "Error" || !destroyOnHit)
        {
            effectTimer -= Time.deltaTime;
            if (effectTimer > 0.0f) { return; }
            effectTimer = effectDelay;
        }

        hitCount = Physics.OverlapCapsuleNonAlloc(point0.position, point1.position, 
                                                   damageRadius, enemyColliders, enemyLayer, QueryTriggerInteraction.Collide);

        for (int i = 0; i < hitCount; i++)
        {
            if (enemyColliders[i].gameObject != gameObject)
            {
                HitEnemy(enemyColliders[i], damage, effectDamage, effectBuildUp, spellType, 
                         effectObjectPoolName, effectInstanceAmount, transform, out gotAHit, true);
            }
        }

        if (stationarySpellPoolName != "Error")
        {
            SpawnStationary();
        }
        
        if(hitCount <= 0)
        {
            hitCount = Physics.OverlapCapsuleNonAlloc(point0.position, point1.position, damageRadius * 2, 
                                                                     terrainColliders, terrainLayer, QueryTriggerInteraction.Collide);

            if (hitCount > 0)
            {
                gotAHit = true;
            }
        }

        if (gotAHit && visualEffectPoolName != "Error")
        {
            if (!destroyOnHit)
            {
                effectTimer -= Time.deltaTime;

                if (effectTimer <= 0.0f)
                {
                    effectTimer = effectDelay;

                    SpawnVFX();
                }
            }
            else
            {
                SpawnVFX();
            }

            if (destroyOnHit)
            {
                pool.Release(this);
            }          
        }       
    }

    /// <summary>
    /// Made by Anton L.
    /// Damages the first object the spell hits if it has the  or the IMagicEffect interfaces.
    /// </summary>
    /// <param name="other">The object the spell hit</param>
    private void InstantCheckHits(Collider other)
    {
        HitEnemy(other, damage, effectDamage, effectBuildUp, spellType, effectObjectPoolName, effectInstanceAmount, 
                                                                                                  transform, out gotAHit, true);

        if (stationarySpellPoolName != "Error")
        {

            SpawnStationary();
        }
        
        if(other.gameObject.layer == terrainLayer)
        {
            gotAHit = true;
        }

        if (gotAHit && destroyOnHit)
        {
            pool.Release(this);
        }
    }

    /// <summary>
    /// Uses an Overlap Sphere to deal damage to any objects it hits and that implement the IDamageable or the IMagicEffect interfaces.
    /// Made by Anton L.
    /// </summary>
    private void OverlapHit()
    {
        checkTimer -= Time.deltaTime;
        if (checkTimer > 0.0f) { return; }
        checkTimer = checkTime;

        count = Physics.OverlapSphereNonAlloc(spherePosition.position, range, colliders, enemyLayer, QueryTriggerInteraction.Collide);
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    HitEnemy(colliders[i], damage, effectDamage, effectBuildUp, spellType, effectObjectPoolName, 
                                                                                   effectInstanceAmount, transform, out gotAHit, true);
                }
            }
        }
    }

    /// <summary>
    /// Addes some force to the object.
    /// </summary>
    /// <param name="position">Unused</param>
    /// <param name="rotation">Unused</param>
    /// <param name="direction">The direction it is shot in</param>
    /// <param name="pool">Unused</param>
    public override void Initialize(Vector3 position, Quaternion rotation, Vector3 direction, IObjectPool<Pooling_Object> pool)
    {
        rb.AddForce(direction, ForceMode.Impulse);
    }

    /// <summary>
    /// Draws the areas where the spell can deal damage and where it can spawn a stationary spell.
    /// </summary>
    
    /*
    private void OnDrawGizmosSelected()
    {
        Extra_Gizmos.DrawCapsule(point0.position, point1.position, damageRadius, capsuleColor);

        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, 
                           transform.position.y - (spawnStationaryRange - spawnStationaryRangeAboveTransform), transform.position.z));

        Gizmos.DrawLine(transform.position, transform.position + Vector3.forward * -spawnOffset);
        Gizmos.DrawLine(transform.position + Vector3.right * 2, transform.position + Vector3.up * spawnStationaryRangeAboveTransform
                                                                                                                 + Vector3.right * 2);

        Gizmos.color = Color.yellow;

        if(useOverlapSphere)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(spherePosition.position, range);
        }       
    }
    */

    /// <summary>
    /// Hits an enemy.
    /// </summary>
    /// <param name="enemy">The collider to hit</param>
    /// <param name="damage">How much damage to deal</param>
    /// <param name="effectDamage">How much effect damage to deal</param>
    /// <param name="effectBuildUp">How much effect build up to apply.</param>
    /// <param name="spellType">What spell type to use</param>
    /// <param name="effectObjectPoolName">Which poo,to grab an effect from</param>
    /// <param name="effectInstanceAmount">the amount of effects used</param>
    /// <param name="spellPosition">The position of the spell</param>
    /// <param name="gotAHit">Is used to record wheter or not something was hit</param>
    public static void HitEnemy(Collider enemy, int damage, int effectDamage, int effectBuildUp, Spell.SpellType spellType,
                   string effectObjectPoolName, int effectInstanceAmount, Transform spellPosition, out bool gotAHit, bool usesEffects)
    {
        IMagicEffect magicEffect = enemy.transform.GetComponent<IMagicEffect>();

        magicEffect?.ApplyMagicEffect(effectDamage, effectBuildUp, spellType);

        IDamageable damagable = enemy.transform.GetComponent<IDamageable>();

        bool gotAKill = false;

        if (damagable != null)
        {
            gotAKill = (bool)(damagable?.TryToDestroyDamageable(damage, spellType));
        }

        if (gotAKill)
        {
            Player_Health.killCount++;
        }

        if (usesEffects)
        {
            if (effectObjectPoolName != "Error")
            {
                for (int x = 0; x < effectInstanceAmount; x++)
                {
                    Pooling_Object pooling_Object = Object_Pooler.Pools[effectObjectPoolName].Get();

                    pooling_Object.Initialize(spellPosition.position, Quaternion.identity,
                                                 enemy.transform.position, Object_Pooler.Pools[effectObjectPoolName]);
                }
            }
        }       

        gotAHit = true;
    }

    /// <summary>
    /// Spawns a stationary.
    /// </summary>
    private void SpawnStationary()
    {
        if (Physics.Raycast(transform.position + Vector3.up * spawnStationaryRangeAboveTransform,
                                                            Vector3.down, out RaycastHit hitInfo, spawnStationaryRange, terrainLayer))
        {
            Spell_Stationary stationary = (Spell_Stationary)stationarySpellPool.Get();

            //Rotates it along the ground.
            Quaternion rotation = Quaternion.FromToRotation(transform.up, hitInfo.normal) * transform.rotation;


            stationary.Initialize(hitInfo.point, rotation, stationarySpellPool);

            gotAHit = true;
        }
    }

    /// <summary>
    /// Spawns a VFX.
    /// </summary>
    private void SpawnVFX()
    {
        Pooled_VFX vfx = (Pooled_VFX)Object_Pooler.Pools[visualEffectPoolName].Get();

        vfx.Initialize(vfxSpawnPos.position, transform.rotation, Vector3.zero, Object_Pooler.Pools[visualEffectPoolName]);
    }
}
