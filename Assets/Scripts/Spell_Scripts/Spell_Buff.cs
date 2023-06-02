using UnityEngine;

[CreateAssetMenu(fileName = "New Spell Buff", menuName = "Spell Buff")]
public class Spell_Buff : Spell
{
    /// <summary>
    /// Casts a spell which gives the player a heplfull buff effect.
    /// </summary>
    /// <param name="player_Look">Reference to where the player is looking</param>
    /// <param name="position">Position the vfx will be spawned at</param>
    /// <param name="rotation">Rotation the vfx will be given</param>
    /// <param name="direction">Direction player is moving in</param>
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

    /// <summary>
    /// Plays a vfx.
    /// </summary>
    /// <param name="position">Position it will be spawned at</param>
    /// <param name="rotation">Rotation it will be given</param>
    /// <param name="direction">Direction player is moving in</param>
    private void PlayEffect(Vector3 position, Quaternion rotation, Vector3 direction)
    {
        if (objectPoolName != "Error")
        {
            Pooled_VFX vfx = (Pooled_VFX)Object_Pooler.Pools[objectPoolName].Get();

            vfx.Initialize(position, rotation, direction, Object_Pooler.Pools[objectPoolName]);
        }
    }
}
