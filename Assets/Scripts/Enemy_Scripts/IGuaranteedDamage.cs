using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGuaranteedDamage
{
    // Made by Daniel.

    public bool GuaranteedDamage(int damage, Spell.SpellType? spellType);
}
