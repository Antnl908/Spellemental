using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spell_Projectile : MonoBehaviour
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

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(int damage, int effectDamage, int effectBuildUp, Spell.SpellType spellType, Vector3 direction)
    {
        rb = GetComponent<Rigidbody>();

        this.damage = damage;
        this.spellType = spellType;
        this.effectDamage = effectDamage;
        this.effectBuildUp = effectBuildUp;

        rb.AddForce(direction, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
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

                damagable?.TakeDamage(damage, spellType);

                IMagicEffect magicEffect = colliders[i].transform.GetComponent<IMagicEffect>();

                magicEffect?.ApplyMagicEffect(effectDamage, effectBuildUp, spellType);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(point0.position, damageRadius);

        Gizmos.DrawSphere(point1.position, damageRadius);
    }
}
