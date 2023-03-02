using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    // Made by Daniel.

    public void TakeDamage(int damage, Spell.SpellType? spellType);

    public void KnockBack(float knockBack);
}
