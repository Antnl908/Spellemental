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
    private Transform spellSpawn;

    [SerializeField]
    private Player_Look player_Look;

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

    private void LateUpdate()
    {
        transform.rotation = player_Look.VirtualCamera.transform.rotation;

        leftHand.transform.rotation = player_Look.VirtualCamera.transform.rotation;
        rightHand.transform.rotation = player_Look.VirtualCamera.transform.rotation;
    }

    private void CastCombinationSpell(InputAction.CallbackContext context)
    {
        foreach(var recipe in recipes)
        {
            if(recipe.SpellMatchesRecipe(leftHand.ActiveSpell, rightHand.ActiveSpell) || 
                                                                             recipe.SpellMatchesRecipe(rightHand.ActiveSpell, leftHand.ActiveSpell))
            {
                recipe.ReturnedSpell.CastSpell(spellSpawn.position, Quaternion.Euler(transform.eulerAngles), transform.forward);

                return;
            }
        }
    }
}
