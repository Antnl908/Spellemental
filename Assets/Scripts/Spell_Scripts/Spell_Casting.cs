using System.Collections;
using System.Collections.Generic;
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
    private List<Spell_Recipe> recipes;

    private Player_Controls controls;

    // Start is called before the first frame update
    void Start()
    {
        controls = new();

        controls.Player1.LeftSpell.performed += leftHand.CastActiveSpell;
        controls.Player1.RightSpell.performed += rightHand.CastActiveSpell;
        controls.Player1.CombineSpell.performed += CastCombinationSpell;

        controls.Player1.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CastCombinationSpell(InputAction.CallbackContext context)
    {
        foreach(var recipe in recipes)
        {
            if(recipe.SpellMatchesRecipe(leftHand.ActiveSpell, rightHand.ActiveSpell) || 
                                                                             recipe.SpellMatchesRecipe(rightHand.ActiveSpell, leftHand.ActiveSpell))
            {
                recipe.ReturnedSpell.CastSpell();

                return;
            }
        }
    }
}
