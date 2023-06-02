using UnityEngine;

public class Cast_Spell_On_Animation : MonoBehaviour
{
    [SerializeField]
    private Spell_Casting caster;

    /// <summary>
    /// This method can be used to cast a spell on an animation event.
    /// </summary>
    public void CastComboSpell()
    {
        caster.Cast();
    }
}
