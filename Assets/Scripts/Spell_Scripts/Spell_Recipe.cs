using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell Recipe", menuName = "Spell Recipe")]
public class Spell_Recipe : ScriptableObject
{
    //Made by Daniel.

    [SerializeField]
    private Spell requiredSpell1;
    [SerializeField]
    private Spell requiredSpell2;

    [SerializeField]
    private Spell returnedSpell;

    public Spell ReturnedSpell { get => returnedSpell; }

    public bool SpellMatchesRecipe(Spell inSpell1, Spell inSpell2)
    {
        if(inSpell1.Type == requiredSpell1.Type && inSpell2.Type == requiredSpell2.Type)
        {
            return true;
        }

        return false;
    }
}
