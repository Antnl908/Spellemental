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
    public virtual void KnockBack(float knockBack)
    {

    }

    public virtual bool TryToDestroyDamageable(int damage, Spell.SpellType? spellType)
    {
        //if (!repeatable && activated) { return false; }
        if (activated) { return false; }

        if(spellType == localSpellType) { return false; }
        activated = true;
        ExecuteInteraction(damage, spellType);
        //activated = true;
        //Debug.Log("Activated");
        //Debug.Log("Activated: " + $"{spellType}");
        return false;
    }

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
