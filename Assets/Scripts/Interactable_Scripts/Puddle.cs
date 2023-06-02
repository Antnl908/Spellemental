using UnityEngine;

public class Puddle : Interactable, IGuaranteedDamage, IDamageable
{
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

    /// <summary>
    /// Gives the puddle a new damage type and a new amount of damage.
    /// </summary>
    /// <param name="damage">How much damage the puddle gets</param>
    /// <param name="spellType">What damage type the puddle gets</param>
    /// <returns></returns>
    public override bool TryToDestroyDamageable(int damage, Spell.SpellType? spellType)
    {

        if (spellType == localSpellType || spellType == lastCapturedSpellType) { return false; }

        lastCapturedSpellType = (Spell.SpellType)spellType;
        lastCapturedDamage = damage;

        return false;
    }

    private void Update()
    {
        //Deals damage to neearby enemies and puddles if possible.
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

    /// <summary>
    /// Gives the puddle a new damage type and a new amount of damage.
    /// </summary>
    /// <param name="damage">How much damage the puddle gets</param>
    /// <param name="spellType">What damage type the puddle gets</param>
    /// <returns></returns>
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
