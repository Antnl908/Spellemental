using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Spell_Casting : MonoBehaviour
{
    //Made by Daniel.

    [SerializeField]
    private Spell_Hand leftHand;

    [SerializeField]
    private Spell_Hand rightHand;

    [SerializeField]
    private Transform spellSpawn;

    [SerializeField]
    private Player_Look player_Look;

    [SerializeField]
    private List<Spell_Recipe> recipes;

    private int spellIndex;

    [SerializeField]
    private TextMeshProUGUI cooldownText;

    [Serializable]
    public class HandColorsForSpells
    {
        [SerializeField]
        private Spell.SpellType leftHandSpell;
        [SerializeField]
        private Spell.SpellType rightHandSpell;

        public Spell.SpellType Left { get => leftHandSpell; }

        public Spell.SpellType Right { get => rightHandSpell; }

        [SerializeField]
        private Color handColor;

        public Color HandColor { get => handColor; }
    }

    [SerializeField]
    private List<HandColorsForSpells> handColors;

    private Color colorOnHands;

    private Spell.SpellType[] handSpellTypes = null;

    private Spell beamSpell;

    private readonly List<bool> spellsCanBeCast = new();
    private readonly List<float> spellCooldowns = new();

    private Player_Controls controls;

    // Start is called before the first frame update
    void Start()
    {
        controls = new();

        controls.Player1.LeftSpell.performed += leftHand.CastActiveSpell;
        controls.Player1.LeftSpell.performed += SetCastingToFalse;
        controls.Player1.SwapLeftSpell.performed += leftHand.CycleSpell;

        controls.Player1.RightSpell.performed += rightHand.CastActiveSpell;
        controls.Player1.RightSpell.performed += SetCastingToFalse;
        controls.Player1.SwapRightSpell.performed += rightHand.CycleSpell;

        controls.Player1.CombineSpell.performed += CastCombinationSpell;

        controls.Player1.Enable();

        leftHand.Player_Look = player_Look;
        rightHand.Player_Look = player_Look;

        SetHandColor(this, EventArgs.Empty);

        leftHand.OnSwitchedSpell += SetHandColor;
        rightHand.OnSwitchedSpell += SetHandColor;

        for(int i = 0; i < recipes.Count; i++)
        {
            if (recipes[i].ReturnedSpell.IsBeam)
            {
                spellsCanBeCast.Add(false);
            }
            else
            {
                spellsCanBeCast.Add(true);
            }

            spellCooldowns.Add(recipes[i].ReturnedSpell.CooldownTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0)
        {
            for (int i = 0; i < recipes.Count; i++)
            {
                if (recipes[i].ReturnedSpell.IsBeam)
                {
                    if (spellsCanBeCast[i] == true)
                    {
                        spellCooldowns[i] -= Time.deltaTime;

                        if (spellCooldowns[i] <= 0 && recipes[i].ReturnedSpell == beamSpell)
                        {
                            spellCooldowns[i] = beamSpell.CooldownTime;

                            beamSpell.CastSpell(player_Look, spellSpawn.position, Quaternion.Euler(transform.eulerAngles), transform.forward);
                        }
                    }
                }
                else
                {
                    if (spellsCanBeCast[i] == false)
                    {
                        spellCooldowns[i] -= Time.deltaTime;

                        if (spellCooldowns[i] <= 0)
                        {
                            spellsCanBeCast[i] = true;
                        }
                    }
                }
            }
        }

        cooldownText.text = Math.Round(spellCooldowns[spellIndex], 2).ToString();
    }

    private void LateUpdate()
    {
        transform.rotation = player_Look.VirtualCamera.transform.rotation;
    }

    private void CastCombinationSpell(InputAction.CallbackContext context)
    {
        if(Time.timeScale > 0)
        {
            foreach (var recipe in recipes)
            {
                if (recipe.SpellMatchesRecipe(leftHand.ActiveSpell, rightHand.ActiveSpell) ||
                                                               recipe.SpellMatchesRecipe(rightHand.ActiveSpell, leftHand.ActiveSpell))
                {
                    int index = recipes.IndexOf(recipe);

                    if (recipe.ReturnedSpell.IsBeam)
                    {
                        spellsCanBeCast[index] = !spellsCanBeCast[index];
                        beamSpell = recipe.ReturnedSpell;
                    }
                    else
                    {
                        if (spellsCanBeCast[index])
                        {
                            recipe.ReturnedSpell.CastSpell(player_Look, spellSpawn.position, Quaternion.Euler(transform.eulerAngles), 
                                                                                                                    transform.forward);

                            spellsCanBeCast[index] = false;

                            spellCooldowns[index] = recipe.ReturnedSpell.CooldownTime;
                        }
                    }

                    spellIndex = index;

                    leftHand.SetCastingToFalse();
                    rightHand.SetCastingToFalse();

                    return;
                }
            }
        }
    }

    private void SetCastingToFalse(InputAction.CallbackContext context)
    {
        if (recipes[spellIndex].ReturnedSpell.IsBeam)
        {
            spellsCanBeCast[spellIndex] = false;
        }
    }

    public void SetHandColor(object sender, EventArgs e)
    {
        if(handSpellTypes == null)
        {
            handSpellTypes = new Spell.SpellType[] { leftHand.ActiveSpell.Type, rightHand.ActiveSpell.Type };
        }
        else
        {
            handSpellTypes[0] = leftHand.ActiveSpell.Type;
            handSpellTypes[1] = rightHand.ActiveSpell.Type;
        }

        for (int i = 0; i < handColors.Count; i++)
        {
            if ((handSpellTypes[0] == handColors[i].Left && handSpellTypes[1] == handColors[i].Right) ||
                (handSpellTypes[1] == handColors[i].Left && handSpellTypes[0] == handColors[i].Right))
            {
                colorOnHands = handColors[i].HandColor;
            }
        }
    }

    public Color HandColor()
    {
        return colorOnHands;
    }
}
