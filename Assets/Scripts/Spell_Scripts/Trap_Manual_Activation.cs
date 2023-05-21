using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Manual_Activation : MonoBehaviour, IDamageable
{
    [SerializeField]
    private Spell.SpellType spellActivatingThisTrap = Spell.SpellType.None;

    [SerializeField]
    private Spell_Stationary trap;

    public void KnockBack(float knockBack)
    {
        //throw new System.NotImplementedException();
    }

    public bool TryToDestroyDamageable(int damage, Spell.SpellType? spellType)
    {
        if(spellType == spellActivatingThisTrap && trap.isActiveAndEnabled)
        {
            trap.SetGotHitToTrue();

            trap.CheckHits();
        }

        return false;
    }
}
