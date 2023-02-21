using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Spell_Hand : MonoBehaviour
{
    //Made by Daniel.

    [SerializeField]
    private List<Spell> spells;

    private int activeSpellIndex = 0;

    public Spell ActiveSpell { get => spells[activeSpellIndex]; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CastActiveSpell(InputAction.CallbackContext context)
    {
        ActiveSpell.CastSpell();
    }

    public void CycleSpell(int stepsCycledAhead)
    {
        activeSpellIndex += stepsCycledAhead;

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
}
