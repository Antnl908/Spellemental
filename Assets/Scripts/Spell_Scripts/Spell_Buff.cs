using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "New Spell Buff", menuName = "Spell Buff")]
public class Spell_Buff : Spell
{
    public override void CastSpell(Player_Look player_Look, Vector3 position, Quaternion rotation, Vector3 direction)
    {
        if(Type == SpellType.Earth)
        {
            Player_Health.GiveDefenseBuff();

            PlayEffect(position, rotation, direction);
        }
        else if(Type == SpellType.Wind)
        {
            Player_Health.GiveHealthBuff();

            PlayEffect(position, rotation, direction);
        }
    }

    private void PlayEffect(Vector3 position, Quaternion rotation, Vector3 direction)
    {
        if (objectPoolName != "Error")
        {
            Pooled_VFX vfx = (Pooled_VFX)Object_Pooler.Pools[objectPoolName].Get();

            vfx.Initialize(position, rotation, direction, Object_Pooler.Pools[objectPoolName]);
        }
    }
}
