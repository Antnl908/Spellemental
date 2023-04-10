using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [SerializeField]
    private TextMeshProUGUI cooldownText;

    private readonly List<bool> spellsCanBeCast = new();
    private readonly List<float> spellCooldowns = new();

    public event EventHandler OnSwitchedSpell;

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

        for(int i = 0; i < Spells.Count; i++)
        {
            if (Spells[i].IsBeam)
            {
                spellsCanBeCast.Add(false);
            }
            else
            {
                spellsCanBeCast.Add(true);
            }

            spellCooldowns.Add(Spells[i].CooldownTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale > 0)
        {
            for (int i = 0; i < Spells.Count; i++)
            {
                if (Spells[i].IsBeam)
                {
                    if(spellsCanBeCast[i] == true)
                    {
                        spellCooldowns[i] -= Time.deltaTime;

                        if (spellCooldowns[i] <= 0 && i == activeSpellIndex)
                        {
                            spellCooldowns[i] = Spells[i].CooldownTime;

                            ActiveSpell.CastSpell(player_Look, spellSpawn.position, Quaternion.Euler(transform.eulerAngles), transform.forward);
                        }
                    }
                }
                else
                {
                    if(spellsCanBeCast[i] == false)
                    {
                        spellCooldowns[i] -= Time.deltaTime;

                        if(spellCooldowns[i] <= 0)
                        {
                            spellsCanBeCast[i] = true;
                        }
                    }
                }
            }
        }

        cooldownText.text = Math.Round(spellCooldowns[activeSpellIndex], 2).ToString();
    }

    public void CastActiveSpell(InputAction.CallbackContext context)
    {
        if(Time.timeScale > 0)
        {
            if (ActiveSpell.IsBeam)
            {
                spellsCanBeCast[activeSpellIndex] = !spellsCanBeCast[activeSpellIndex];
            }
            else
            {
                if (spellsCanBeCast[activeSpellIndex] == true)
                {
                    ActiveSpell.CastSpell(player_Look, spellSpawn.position, Quaternion.Euler(transform.eulerAngles), transform.forward);

                    spellsCanBeCast[activeSpellIndex] = false;

                    spellCooldowns[activeSpellIndex] = ActiveSpell.CooldownTime;
                }
            }
        }
    }

    public void CycleSpell(InputAction.CallbackContext context)
    {
        activeSpellIndex++;

        WrapSpellIndex();

        SetSpellEffect();

        OnSwitchedSpell?.Invoke(this, EventArgs.Empty);
    }

    public void SetSpellIndex(int index)
    {
        activeSpellIndex = index;

        WrapSpellIndex();

        SetSpellEffect();

        OnSwitchedSpell?.Invoke(this, EventArgs.Empty);
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
        if (ActiveSpell.IsBeam)
        {
            spellsCanBeCast[activeSpellIndex] = false;
        }
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
