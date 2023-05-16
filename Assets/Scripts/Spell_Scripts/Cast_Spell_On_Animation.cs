using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cast_Spell_On_Animation : MonoBehaviour
{
    [SerializeField]
    private Spell_Casting caster;

    public void CastComboSpell()
    {
        caster.Cast();
    }
}
