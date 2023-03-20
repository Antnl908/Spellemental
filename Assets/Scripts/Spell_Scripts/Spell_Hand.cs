using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Spell_Hand : MonoBehaviour
{
    //Made by Daniel.

    [SerializeField]
    private List<Spell> spells;

    [SerializeField]
    private Transform spellSpawn;

    private int activeSpellIndex = 0;

    public Spell ActiveSpell { get => spells[activeSpellIndex]; }

    private bool isCasting = false;

    private const float castTime = 0.1f;

    private float timeUntilCast = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isCasting)
        {
            timeUntilCast -= Time.deltaTime;

            if(timeUntilCast <= 0 )
            {
                ActiveSpell.CastSpell(spellSpawn.position, Quaternion.Euler(transform.eulerAngles), transform.forward);

                timeUntilCast = castTime;
            }
        }
    }

    public void CastActiveSpell(InputAction.CallbackContext context)
    {
        if (ActiveSpell.IsBeam)
        {
            isCasting = !isCasting;
        }
        else
        {
            ActiveSpell.CastSpell(spellSpawn.position, Quaternion.Euler(transform.eulerAngles), transform.forward);
        }
    }

    public void CycleSpell(InputAction.CallbackContext context)
    {
        activeSpellIndex++;

        WrapSpellIndex();
    }

    public void SetSpellIndex(int index)
    {
        activeSpellIndex = index;

        WrapSpellIndex();
    }

    private void WrapSpellIndex()
    {
        if (activeSpellIndex >= spells.Count)
        {
            activeSpellIndex = 0;
        }
    }

    public void SetCastingToFalse()
    {
        isCasting = false;
        timeUntilCast = 0;
    }
}
