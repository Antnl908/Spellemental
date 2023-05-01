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

    [SerializeField]
    private Spell_Casting caster;

    private Player_Look player_Look;

    private int activeSpellIndex = 0;

    public Spell ActiveSpell { get => spells[activeSpellIndex]; }

    public event EventHandler OnSwitchedSpell;

    private bool isCasting = false;

    public bool IsCasting { get => isCasting; set { isCasting = value; } }

    [SerializeField]
    private Spell_Hand otherHand;

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

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private bool isLeftHand = true;

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
        
    }

    public void CastActiveSpell(InputAction.CallbackContext context)
    {
        if(Time.timeScale > 0)
        {
            if(!otherHand.IsCasting && !caster.IsCasting)
            {
                isCasting = true;

                if(isLeftHand)
                {
                    animator.SetBool("StartLeftCast", true);
                }
                else
                {
                    animator.SetBool("StartRightCast", true);
                }

                StartCoroutine(UseSpell());
            }
        }
    }

    private IEnumerator UseSpell()
    {
        while(isCasting)
        {
            if (isLeftHand)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("LeftHold"))
                {
                    if (caster.CurrentMana >= ActiveSpell.ManaCost)
                    {
                        ActiveSpell.CastSpell(player_Look, spellSpawn.position, Quaternion.Euler(transform.eulerAngles), transform.forward);

                        caster.AlterMana(-ActiveSpell.ManaCost);

                        yield return new WaitForSeconds(ActiveSpell.TimeBetweenCasts);
                    }
                    else
                    {
                        yield return null;
                    }
                }
                else
                {
                    yield return null;
                }
            }
            else
            {
                if (animator.GetCurrentAnimatorStateInfo(1).IsName("RightHold"))
                {
                    if (caster.CurrentMana >= ActiveSpell.ManaCost)
                    {
                        ActiveSpell.CastSpell(player_Look, spellSpawn.position, Quaternion.Euler(transform.eulerAngles), transform.forward);

                        caster.AlterMana(-ActiveSpell.ManaCost);

                        yield return new WaitForSeconds(ActiveSpell.TimeBetweenCasts);
                    }
                    else
                    {
                        yield return null;
                    }
                }
                else
                {
                    yield return null;
                }
            }        
            
        }
    }

    public void QuitCasting(InputAction.CallbackContext context)
    {
        isCasting = false;

        if(isLeftHand)
        {
            animator.SetBool("StartLeftCast", false);
        }
        else
        {
            animator.SetBool("StartRightCast", false);
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
