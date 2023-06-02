using System;
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

    [SerializeField]
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

    private readonly Collider[] enemyColliders = new Collider[100];

    private int hitCount;

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

    [SerializeField]
    private bool bouncesPlayer = false;

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

            if (useOverlapSphere) 
            { 
                OverlapHit(); 
            }

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

    /// <summary>
    /// Initializes the spells starting values.
    /// </summary>
    /// <param name="position">The position where it is placed</param>
    /// <param name="rotation">The starting rotation</param>
    /// <param name="pool">The pool the spell came from</param>
    /// <param name="damage">How much damage it will deal</param>
    /// <param name="effectDamage">How much effect damage it will deal</param>
    /// <param name="effectBuildUp">How many percentages of effect build up the spell will give</param>
    /// <param name="spellType">The spells spell type</param>
    /// <param name="destructionTime">How long it is until the spell is returned to the pool</param>
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

    /// <summary>
    /// Sets the position, roation and pool of the spell.
    /// </summary>
    /// <param name="position">The position of the spell</param>
    /// <param name="rotation">The rotation of the spell</param>
    /// <param name="pool">The pool the spell came from</param>
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
            if (other.gameObject.TryGetComponent<Player_Move>(out var player))
            {
                player.Bounce();

                SetGotHitToTrue();
            }

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

    /// <summary>
    /// Deals damage to any object that implements the IDamageable or the IMagicEffect interfaces. 
    /// Spawns a vfx if it destroys upon hitting something.
    /// </summary>
    public void CheckHits()
    {
        if (!isActiveAndEnabled)
        {
            return;
        }

        if (!destroyOnHit)
        {
            effectTimer -= Time.deltaTime;
            if (effectTimer > 0.0f) { return; }
            effectTimer = effectDelay;
        }

        hitCount = Physics.OverlapBoxNonAlloc(transform.position + hitBoxOffset, new Vector3(width / 2, height / 2, depth / 2),
                                                   enemyColliders, transform.rotation, enemyLayer, QueryTriggerInteraction.Collide);

        for (int i = 0; i < hitCount; i++)
        {
            if (enemyColliders[i].gameObject != gameObject && hitDelay <= 0)
            {
                Spell_Projectile.HitEnemy(enemyColliders[i], damage, effectDamage, effectBuildUp, spellType, "0", 0,
                                                                                      transform, out gotAHit, false);
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

    /// <summary>
    /// Sets that the spell hit something.
    /// </summary>
    public void SetGotHitToTrue()
    {
        gotAHit = true;
    }

    /// <summary>
    /// Made by Anton L.
    /// Damages the first object the spell hits if it has the  or the IMagicEffect interfaces.
    /// </summary>
    /// <param name="other">The object the spell hit</param>
    private void InstantCheckHits(Collider other)
    {
        Spell_Projectile.HitEnemy(other, damage, effectDamage, effectBuildUp, spellType, "0", 0,
                                                                                      transform, out gotAHit, false);

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

        count = Physics.OverlapSphereNonAlloc(transform.position, range, colliders, enemyLayer, QueryTriggerInteraction.Collide);
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    Spell_Projectile.HitEnemy(colliders[i], damage, effectDamage, effectBuildUp, spellType, "0", 0, 
                                                                                      transform, out gotAHit, false);
                }
            }
        }
    }

    /// <summary>
    /// Draws the areas where the spell can damage enemies.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position + hitBoxOffset, new Vector3(width / 2, height / 2, depth / 2));

        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y + heightOffset, 
                                                                                                      transform.position.z));

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
