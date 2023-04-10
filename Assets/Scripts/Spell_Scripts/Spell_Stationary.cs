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

        Initialize(position, rotation, pool);
    }

    public void Initialize(Vector3 position, Quaternion rotation, IObjectPool<Pooling_Object> pool)
    {
        //Has to be done in this order for it to work.
#pragma warning disable UNT0022 // Inefficient position/rotation assignment
        transform.rotation = rotation;
#pragma warning restore UNT0022 // Inefficient position/rotation assignment

        transform.position = position + transform.up * heightOffset;

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

    private void CheckHits()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(width / 2, height / 2, depth / 2), transform.rotation, enemyLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                IMagicEffect magicEffect = colliders[i].transform.GetComponent<IMagicEffect>();

                magicEffect?.ApplyMagicEffect(effectDamage, effectBuildUp, spellType);

                IDamageable damagable = colliders[i].transform.GetComponent<IDamageable>();

                bool gotAKill = (bool)(damagable?.TryToDestroyDamageable(damage, spellType));

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

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(width / 2, height / 2, depth / 2));

        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y + heightOffset, 
                                                                                                      transform.position.z));
    }
}
