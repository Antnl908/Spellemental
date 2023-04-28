using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado_Health : MonoBehaviour, IDamageable
{
    //Made by Daniel.

    [SerializeField]
    private int fireExplosionDamage = 20;

    [SerializeField]
    private int iceExplosionDamage = 20;

    [SerializeField]
    private int lightningExplosionDamage = 20;

    [SerializeField]
    private float range = 3;

    [SerializeField]
    private LayerMask enemyMask;

    [SerializeField]
    private Spell_Stationary stationary;

    [SerializeField]
    private int fireDamageThreshold = 4;

    [SerializeField]
    private int iceDamageThreshold = 4;

    [SerializeField]
    private int lightningDamageThreshold = 3;

    public void KnockBack(float knockBack)
    {
        Debug.Log("No knockback on tornadoes!");
    }

    public bool TryToDestroyDamageable(int damage, Spell.SpellType? spellType)
    {
        if(spellType == Spell.SpellType.Fire && damage >= fireDamageThreshold)
        {
            CheckHits(fireExplosionDamage, (Spell.SpellType)spellType);
        }
        else if(spellType == Spell.SpellType.Ice && damage >= iceDamageThreshold)
        {
            CheckHits(iceExplosionDamage, (Spell.SpellType)spellType);
        }
        else if(spellType == Spell.SpellType.Lightning && damage >= lightningDamageThreshold)
        {
            CheckHits(lightningExplosionDamage, (Spell.SpellType)spellType);
        }

        return false;
    }

    private void CheckHits(int damage, Spell.SpellType spellType)
    {
        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, range, enemyMask);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                IGuaranteedDamage damagable = colliders[i].transform.GetComponent<IGuaranteedDamage>();

                bool gotAKill = false;

                if (damagable != null)
                {
                    gotAKill = (bool)(damagable?.GuaranteedDamage(damage, spellType));
                }

                if (gotAKill)
                {
                    Player_Health.killCount++;
                }
            }
        }

        stationary.Pool.Release(stationary);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;    
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
