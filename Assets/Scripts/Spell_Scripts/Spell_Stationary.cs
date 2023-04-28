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

    // Update is called once per frame
    void Update()
    {
        if (destroyedAfterSomeTime && enabled)
        {
            currentDestructionTime -= Time.deltaTime;

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

    private void OnTriggerStay(Collider other)
    {
        if(!destroyOnHit)
        {
            CheckHits();
        }       
    }

    private void OnTriggerEnter(Collider other)
    {
        if(destroyOnHit)
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
            pool.Release(this);
        }
    }

    public void SetGotHitToTrue()
    {
        gotAHit = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position + hitBoxOffset, new Vector3(width / 2, height / 2, depth / 2));

        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y + heightOffset, 
                                                                                                      transform.position.z));
    }
}
