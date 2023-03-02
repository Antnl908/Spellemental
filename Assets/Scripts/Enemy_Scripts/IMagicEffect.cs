using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMagicEffect
{
    // Made by Daniel.

    public void ApplyMagicEffect(int effectDamage, int effectBuildUp, Spell.SpellType spellType);

    public void FireEffect(int fireDamage, int effectBuildUp);

    public void FrostEffect(int iceDamage, int effectBuildUp);

    public void SlowDownEffect(float slowDownAmount);
}
