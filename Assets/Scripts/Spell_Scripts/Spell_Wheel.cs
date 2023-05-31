using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

public class Spell_Wheel : MonoBehaviour
{
    //Made by Daniel.

    [SerializeField]
    private Spell_Hand leftHand;

    [SerializeField]
    private Spell_Hand rightHand;

    [SerializeField]
    private Player_Look look;

    private Player_Controls controls;

    [SerializeField]
    private LayerMask spellWheelLayer;

    [SerializeField]
    private GameObject wheel;

    [SerializeField]
    private List<Spell_Select> spellSelects;

    [SerializeField]
    private float rotationAmount;

    [Range(1, 10)]
    [SerializeField]
    private float rangeFromPlayer = 1;

    private bool wasRecentlyActivated = false;

    // Start is called before the first frame update
    void Start()
    {
        spellSelects.AddRange(wheel.GetComponentsInChildren<Spell_Select>());

        controls = new();

        controls.Player1.SpellWheel.performed += ActivateWheel;
        controls.Player1.SpellWheel.canceled += DeactivateWheel;

        controls.Player1.SwapLeftSpell.performed += ActivateLeftHandSpell;
        controls.Player1.LeftTap.performed += ActivateLeftHandSpell;

        controls.Player1.SwapRightSpell.performed += ActivateRightHandSpell;
        controls.Player1.RightTap.performed += ActivateRightHandSpell;

        controls.Player1.Enable();

        DeactivateUnusedSpellSelects();
        ActivateSpellSelects();

        wheel.SetActive(false);
    }

    private void OnDisable()
    {
        controls.Player1.SpellWheel.performed -= ActivateWheel;
        controls.Player1.SpellWheel.canceled -= DeactivateWheel;

        controls.Player1.SwapLeftSpell.performed -= ActivateLeftHandSpell;
        controls.Player1.LeftTap.performed -= ActivateLeftHandSpell;

        controls.Player1.SwapRightSpell.performed -= ActivateRightHandSpell;
        controls.Player1.RightTap.performed -= ActivateRightHandSpell;
    }

    // Update is called once per frame
    void Update()
    {
        if(wheel.activeInHierarchy)
        {
            Vector2 rotation = controls.Player1.Look.ReadValue<Vector2>();

            wheel.transform.Rotate(rotation.y * rotationAmount * Time.unscaledDeltaTime * Vector3.right, Space.Self);
        }

        if (wasRecentlyActivated)
        {
            ActivateSpellSelects();

            DeactivateUnusedSpellSelects();

            wasRecentlyActivated = false;
        }
    }

    private void ActivateLeftHandSpell(InputAction.CallbackContext context)
    {
        ActivateSpells(leftHand, true);
    }

    private void ActivateRightHandSpell(InputAction.CallbackContext context)
    {
        ActivateSpells(rightHand, false);
    }

    //Activates a SPell_Select if its spell matches one of your equipped spells.
    private void ActivateSpells(Spell_Hand hand, bool isLeftSprite)
    {
        if(Time.timeScale == 0)
        {
            foreach (var spellSelect in spellSelects)
            {
                if (spellSelect.SelectIfHit(look.transform.position, spellSelects, isLeftSprite))
                {
                    hand.SetSpellIndex(hand.Spells.IndexOf(spellSelect.SelectedSpell));
                }
            }

            DeactivateUnusedSpellSelects();
        }       
    }

    //Activates the spell wheel, places it in front of you and pauses time.
    private void ActivateWheel(InputAction.CallbackContext context)
    {
        wheel.SetActive(true);

        
        wheel.transform.SetPositionAndRotation(look.VirtualCamera.transform.position + look.VirtualCamera.transform.forward * 
                                                                             rangeFromPlayer, look.VirtualCamera.transform.rotation);

        wasRecentlyActivated = true;

        Time.timeScale = 0;
    }

    //Deactivates the spell wheel and unpauses time.
    private void DeactivateWheel(InputAction.CallbackContext context)
    {
        wheel.SetActive(false);

        Time.timeScale = 1;
    }

    //Gives all Spell_Select their selected sprites if they match any equipped spell.
    private void ActivateSpellSelects()
    {
        foreach (Spell_Select spellSelect in spellSelects)
        {
            if (spellSelect.SelectedSpell == leftHand.ActiveSpell)
            {
                spellSelect.Select(true);
            }

            if(spellSelect.SelectedSpell == rightHand.ActiveSpell)
            {
                spellSelect.Select(false);
            }
        }
    }

    //Gives all Spell_Select their unselected sprites if they do not match any equipped spell.
    private void DeactivateUnusedSpellSelects()
    {
        for (int i = 0; i < spellSelects.Count; i++)
        {
            if (spellSelects[i].SelectedSpell != leftHand.ActiveSpell)
            {
                spellSelects[i].UnSelect(true);
            }

            if(spellSelects[i].SelectedSpell != rightHand.ActiveSpell)
            {
                spellSelects[i].UnSelect(false);
            }
        }
    }
}
