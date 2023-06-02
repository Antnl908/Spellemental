using UnityEngine;

public class Trap_Manual_Activation : MonoBehaviour, IDamageable
{
    [SerializeField]
    private Spell.SpellType spellActivatingThisTrap = Spell.SpellType.None;

    [SerializeField]
    private Spell_Stationary trap;

    /// <summary>
    /// Unused
    /// </summary>
    /// <param name="knockBack">Unused</param>
    public void KnockBack(float knockBack)
    {
        //throw new System.NotImplementedException();
    }

    /// <summary>
    /// Manualy activates the trap if it is hit by a spell with the correct spell type.
    /// </summary>
    /// <param name="damage">Unused</param>
    /// <param name="spellType">Spell type of the spell which hit the trap</param>
    /// <returns>False</returns>
    public bool TryToDestroyDamageable(int damage, Spell.SpellType? spellType)
    {
        if(spellType == spellActivatingThisTrap && trap.isActiveAndEnabled)
        {
            trap.SetGotHitToTrue();

            trap.CheckHits();
        }

        return false;
    }

    /// <summary>
    /// Bounces the player if they hit this trap and then activates this trap.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if(isActiveAndEnabled)
        {
            if (other.gameObject.TryGetComponent<Player_Move>(out var player))
            {
                player.Bounce();

                TryToDestroyDamageable(1, spellActivatingThisTrap);
            }
        }        
    }
}
