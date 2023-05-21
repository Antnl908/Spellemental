using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puddle : Interactable
{
    //public override void ExecuteInteraction(int damage, Spell.SpellType? spellType)
    //{
    //    Debug.Log("Activated: "+$"{spellType}");
    //}

    float coolDown = 1f;
    float timer;

    public override bool TryToDestroyDamageable(int damage, Spell.SpellType? spellType)
    {
        //if (!repeatable && activated) { return false; }
        if (activated) { return false; }

        if (spellType == localSpellType) { return false; }

        //There has to be a better way to avoid infinite loops when testing for chain reactions
        activated = true;
        ExecuteInteraction(damage, spellType);
        //activated = true;
        //Debug.Log("Activated");
        Debug.Log("Activated: " + $"{spellType}");
        return false;
    }

    private void Update()
    {
        if (activated) { timer -= Time.deltaTime; if (timer < 0f) { activated = false; timer = coolDown; } }
    }
}
