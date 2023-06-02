using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.UIElements;

public class Interactable : MonoBehaviour, IDamageable
{
    [SerializeField] private bool repeatable;
    [SerializeField] private float radius;
    readonly Collider[] colliders = new Collider[150];
    [SerializeField] LayerMask layerMask;
    [SerializeField] protected Spell.SpellType localSpellType;
    int count;
    protected bool activated;

    private readonly Collider[] guaranteedColliders = new Collider[150];
    private int guaranteedCount;

    public virtual void KnockBack(float knockBack)
    {

    }

    public virtual bool TryToDestroyDamageable(int damage, Spell.SpellType? spellType)
    {
        if (activated) { return false; }

        if(spellType == localSpellType) { return false; }
        activated = true;
        ExecuteInteraction(damage, spellType);
        
        return false;
    }

    /// <summary>
    /// Attempts to deal damage to any object that has implemented the IDamageable interface and that is hit by the overlap sphere.
    /// </summary>
    /// <param name="damage">How much damage will be dealt</param>
    /// <param name="spellType">Which type of damage will be dealt</param>
    public virtual void ExecuteInteraction(int damage, Spell.SpellType? spellType)
    {
        count = Physics.OverlapSphereNonAlloc(transform.position, radius, colliders, layerMask, QueryTriggerInteraction.Collide);
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                IDamageable damagable = colliders[i].transform.GetComponent<IDamageable>();
                damagable?.TryToDestroyDamageable(damage, spellType);

            }
        }

        activated = true;
    }

    /// <summary>
    /// Attempts to deal damage to any object that has implemented the IGuaranteedDamage interface and that is hit by the overlap sphere.
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="spellType"></param>
    protected void GuaranteedExecuteInteraction(int damage, Spell.SpellType? spellType)
    {
        guaranteedCount = Physics.OverlapSphereNonAlloc(transform.position, radius, guaranteedColliders, layerMask, QueryTriggerInteraction.Collide);
        if (guaranteedCount > 0)
        {
            for (int i = 0; i < guaranteedCount; i++)
            {
                IGuaranteedDamage damagable = guaranteedColliders[i].transform.GetComponent<IGuaranteedDamage>();
                damagable?.GuaranteedDamage(damage, spellType);

            }
        }
    }

    /// <summary>
    /// Draws a blue wire sphere in the unity editor. It represents the area where objects can be hit by an overlap sphere.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
