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

    [SerializeField]
    private int maxMana = 1000;

    private int currentMana;

    public int MaxMana { get => maxMana; }

    public int CurrentMana { get => currentMana; }

    [SerializeField]
    private int manaRegeneration = 10;

    [Serializable]
    public class HandColorsForSpells
    {
        [SerializeField]
        private Spell.SpellType spell;
        public Spell.SpellType Spell { get => spell; }

        [SerializeField]
        private Color handColor;

        public Color HandColor { get => handColor; }
    }

    [SerializeField]
    private List<HandColorsForSpells> rightHandColors;

    [SerializeField]
    private List<HandColorsForSpells> leftHandColors;

    private Color colorOnLeftHand;

    private Color colorOnRightHand;

    private Spell.SpellType[] handSpellTypes = null;

    private bool isCasting = false;

    public bool IsCasting { get => isCasting; }

    private Player_Controls controls;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private TextMeshProUGUI spellCostText;

    private Spell spellToCast;

    // Start is called before the first frame update
    void Start()
    {
        controls = new();

        controls.Player1.LeftSpell.performed += leftHand.CastActiveSpell;
        controls.Player1.LeftSpell.canceled += leftHand.QuitCasting;
        controls.Player1.SwapLeftSpell.performed += leftHand.CycleSpell;

        controls.Player1.RightSpell.performed += rightHand.CastActiveSpell;
        controls.Player1.RightSpell.canceled += rightHand.QuitCasting;
        controls.Player1.SwapRightSpell.performed += rightHand.CycleSpell;

        controls.Player1.CombineSpell.performed += CastCombinationSpell;
        controls.Player1.CombineSpell.canceled += QuitCasting;

        controls.Player1.Enable();

        leftHand.Player_Look = player_Look;
        rightHand.Player_Look = player_Look;

        SetHandColor(this, EventArgs.Empty);

        DisplaySpellCost(this, EventArgs.Empty);

        leftHand.OnSwitchedSpell += SetHandColor;
        leftHand.OnSwitchedSpell += DisplaySpellCost;

        rightHand.OnSwitchedSpell += SetHandColor;
        rightHand.OnSwitchedSpell += DisplaySpellCost;

        currentMana = maxMana;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(Time.timeScale > 0)
        {
            if(currentMana < maxMana)
            {
                AlterMana((int)(manaRegeneration * Time.fixedDeltaTime));
            }            
        }
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
                    isCasting = true;

                    StartCoroutine(UseSpell(recipe.ReturnedSpell));
                }
            }
        }
    }

    private IEnumerator UseSpell(Spell spell)
    {
        while (isCasting)
        {
            if(currentMana >= spell.ManaCost)
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName(spell.LeftAnimationName))
                {
                    animator.SetBool(spell.AnimationBoolName, true);

                    //spell.CastSpell(player_Look, spellSpawn.position, Quaternion.Euler(transform.eulerAngles), transform.forward);

                    //AlterMana(-spell.ManaCost);

                    spellToCast = spell;

                    yield return new WaitForSeconds(spell.TimeBetweenCasts);
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

    public void Cast()
    {
        spellToCast.CastSpell(player_Look, spellSpawn.position, Quaternion.Euler(transform.eulerAngles), transform.forward);

        AlterMana(-spellToCast.ManaCost);
    }

    private void QuitCasting(InputAction.CallbackContext context)
    {
        isCasting = false;
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

        for (int i = 0; i < rightHandColors.Count; i++)
        {
            if (handSpellTypes[1] == rightHandColors[i].Spell)
            {
                colorOnRightHand = rightHandColors[i].HandColor;
            }
        }

        for (int i = 0; i < leftHandColors.Count; i++)
        {
            if (handSpellTypes[0] == leftHandColors[i].Spell)
            {
                colorOnLeftHand = leftHandColors[i].HandColor;
            }
        }
    }

    private void DisplaySpellCost(object sender, EventArgs e)
    {
        foreach(var recipe in recipes)
        {
            if(recipe.SpellMatchesRecipe(leftHand.ActiveSpell, rightHand.ActiveSpell) ||
                                                               recipe.SpellMatchesRecipe(rightHand.ActiveSpell, leftHand.ActiveSpell))
            {
                spellCostText.text = recipe.ReturnedSpell.ManaCost.ToString();
            }
        }
    }

    public void AlterMana(int amount)
    {
        currentMana = Mathf.Clamp(currentMana + amount, 0, maxMana);
    }

    public Color LeftHandColor()
    {
        return colorOnLeftHand;
    }

    public Color RightHandColor()
    {
        return colorOnRightHand;
    }
}
