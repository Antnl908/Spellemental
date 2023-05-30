using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puddle : Interactable, IGuaranteedDamage, IDamageable
{
    //public override void ExecuteInteraction(int damage, Spell.SpellType? spellType)
    //{
    //    Debug.Log("Activated: "+$"{spellType}");
    //}

    float coolDown = 1f;
    float timer;

    private Spell.SpellType lastCapturedSpellType;

    private int lastCapturedDamage;

    private const float timeBetweenHits = 0.5f;

    private float currentTimeBetweenHits = 0;

    private bool canHit = true;

    private void OnEnable()
    {
        lastCapturedSpellType = Spell.SpellType.None;

        lastCapturedDamage = 0;
    }

    public override bool TryToDestroyDamageable(int damage, Spell.SpellType? spellType)
    {
        //if (!repeatable && activated) { return false; }
        //if (activated) { return false; }

        if (spellType == localSpellType || spellType == lastCapturedSpellType) { return false; }

        //There has to be a better way to avoid infinite loops when testing for chain reactions
        //activated = true;
        //ExecuteInteraction(damage, spellType);

        lastCapturedSpellType = (Spell.SpellType)spellType;
        lastCapturedDamage = damage;
        //activated = true;
        //Debug.Log("Activated");
        //Debug.Log("Activated: " + $"{spellType}");
        return false;
    }

    private void Update()
    {
        //if (activated) { timer -= Time.deltaTime; if (timer < 0f) { activated = false; timer = coolDown; } }

        if(lastCapturedSpellType != Spell.SpellType.None)
        {
            if(canHit)
            {
                GuaranteedExecuteInteraction(lastCapturedDamage, lastCapturedSpellType);

                currentTimeBetweenHits = timeBetweenHits;

                canHit = false;
            }
            else
            {
                currentTimeBetweenHits -= Time.deltaTime;

                if (currentTimeBetweenHits <= 0f)
                {
                    canHit = true;
                }
            }
        }
    }

    public bool GuaranteedDamage(int damage, Spell.SpellType? spellType)
    {
        if(spellType == localSpellType || spellType == lastCapturedSpellType) 
        { 
            return false; 
        }

        lastCapturedSpellType = (Spell.SpellType)spellType;
        lastCapturedDamage = damage;

        return false;
    }
}
