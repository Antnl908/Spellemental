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

    private Spell beamSpell;

    private bool isCasting = false;

    private const float castTime = 0.1f;

    private float timeUntilCast = 0;

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

    }

    // Update is called once per frame
    void Update()
    {
        if (isCasting)
        {
            timeUntilCast -= Time.deltaTime;

            if (timeUntilCast <= 0)
            {
                beamSpell.CastSpell(player_Look, spellSpawn.position, Quaternion.Euler(transform.eulerAngles), transform.forward);

                timeUntilCast = castTime;
            }
        }
    }

    private void LateUpdate()
    {
        transform.rotation = player_Look.VirtualCamera.transform.rotation;
    }

    private void CastCombinationSpell(InputAction.CallbackContext context)
    {
        foreach(var recipe in recipes)
        {
            if(recipe.SpellMatchesRecipe(leftHand.ActiveSpell, rightHand.ActiveSpell) || 
                                                                             recipe.SpellMatchesRecipe(rightHand.ActiveSpell, leftHand.ActiveSpell))
            {
                if(recipe.ReturnedSpell.IsBeam)
                {
                    isCasting = !isCasting;
                    beamSpell = recipe.ReturnedSpell;
                }
                else
                {
                    recipe.ReturnedSpell.CastSpell(player_Look, spellSpawn.position, Quaternion.Euler(transform.eulerAngles), transform.forward);
                }

                leftHand.SetCastingToFalse();
                rightHand.SetCastingToFalse();

                return;
            }
        }
    }

    private void SetCastingToFalse(InputAction.CallbackContext context)
    {
        isCasting = false;
        timeUntilCast = 0;
    }
}
