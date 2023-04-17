using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell Buff", menuName = "Spell Buff")]
public class Spell_Buff : Spell
{
    public override void CastSpell(Player_Look player_Look, Vector3 position, Quaternion rotation, Vector3 direction)
    {
        if(Type == SpellType.Earth)
        {
            Player_Health.GiveDefenseBuff();
        }
        else if(Type == SpellType.Wind)
        {
            Player_Health.GiveHealthBuff();
        }
    }
}
