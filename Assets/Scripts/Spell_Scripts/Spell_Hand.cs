using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Spell_Hand : MonoBehaviour
{
    //Made by Daniel.

    [SerializeField]
    private List<Spell> spells;

    public List<Spell> Spells { get => spells; }

    [SerializeField]
    private Transform spellSpawn;

    private Player_Look player_Look;

    private int activeSpellIndex = 0;

    public Spell ActiveSpell { get => spells[activeSpellIndex]; }

    private bool isCasting = false;

    private const float castTime = 0.1f;

    private float timeUntilCast = 0;

    public event Action SwitchedSpellEvent = delegate { };

    [Serializable]
    public class Effect
    {
        [SerializeField]
        private Spell.SpellType spellType;

        public Spell.SpellType SpellType { get { return spellType; } }

        [SerializeField]
        private GameObject spellEffect;

        public GameObject SpellEffect { get { return spellEffect; } }
    }

    [SerializeField]
    private List<Effect> effects;

    private Dictionary<Spell.SpellType, GameObject> spellEffects = new();

    private void Awake()
    {
        foreach(var effect in effects)
        {
            spellEffects.Add(effect.SpellType, effect.SpellEffect);
        }

        SetSpellEffect();
    }

    // Update is called once per frame
    void Update()
    {
        if(isCasting && Time.timeScale > 0)
        {
            timeUntilCast -= Time.deltaTime;

            if(timeUntilCast <= 0 )
            {
                ActiveSpell.CastSpell(Player_Look, spellSpawn.position, Quaternion.Euler(transform.eulerAngles), transform.forward);

                timeUntilCast = castTime;
            }
        }
    }

    public void CastActiveSpell(InputAction.CallbackContext context)
    {
        if(Time.timeScale > 0)
        {
            if (ActiveSpell.IsBeam)
            {
                isCasting = !isCasting;
            }
            else
            {
                ActiveSpell.CastSpell(Player_Look, spellSpawn.position, Quaternion.Euler(transform.eulerAngles), transform.forward);
            }
        }
    }

    public void CycleSpell(InputAction.CallbackContext context)
    {
        activeSpellIndex++;

        WrapSpellIndex();

        SetSpellEffect();

        SwitchedSpellEvent.Invoke();
    }

    public void SetSpellIndex(int index)
    {
        activeSpellIndex = index;

        WrapSpellIndex();

        SetSpellEffect();

        SwitchedSpellEvent.Invoke();
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

    private void SetSpellEffect()
    {
        foreach(var effect in spellEffects)
        {
            effect.Value.SetActive(false);
        }

        spellEffects[ActiveSpell.Type].SetActive(true);
    }

    public Player_Look Player_Look
    {
        get { return player_Look; }
        set { player_Look = value; }
    }
}
