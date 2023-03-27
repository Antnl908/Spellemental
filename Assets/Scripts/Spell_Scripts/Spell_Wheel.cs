using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
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

    // Start is called before the first frame update
    void Start()
    {
        spellSelects.AddRange(wheel.GetComponentsInChildren<Spell_Select>());

        controls = new();

        controls.Player1.SpellWheel.performed += ActivateWheel;
        controls.Player1.SpellWheel.canceled += DeactivateWheel;

        controls.Player1.LeftSpell.performed += ActivateLeftHandSpell;
        controls.Player1.RightSpell.performed += ActivateRightHandSpell;

        controls.Player1.Enable();

        foreach(Spell_Select spellSelect in spellSelects)
        {
            if(spellSelect.SelectedSpell == leftHand.ActiveSpell || spellSelect.SelectedSpell == rightHand.ActiveSpell)
            {
                spellSelect.Select();
            }
        }

        wheel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(wheel.activeInHierarchy)
        {
            Vector2 rotation = controls.Player1.Look.ReadValue<Vector2>();

            wheel.transform.Rotate(rotation.y * rotationAmount * Time.unscaledDeltaTime * Vector3.right, Space.Self);
        }
    }

    private void ActivateLeftHandSpell(InputAction.CallbackContext context)
    {
        ActivateSpells(leftHand);
    }

    private void ActivateRightHandSpell(InputAction.CallbackContext context)
    {
        ActivateSpells(rightHand);
    }

    private void ActivateSpells(Spell_Hand hand)
    {
        if(Time.timeScale == 0)
        {
            foreach (var spellSelect in spellSelects)
            {
                if (spellSelect.SelectIfHit(look.transform.position, spellSelects))
                {
                    hand.SetSpellIndex(hand.Spells.IndexOf(spellSelect.SelectedSpell));
                }
            }

            for (int i = 0; i < spellSelects.Count; i++)
            {
                if (spellSelects[i].SelectedSpell != leftHand.ActiveSpell && spellSelects[i].SelectedSpell != rightHand.ActiveSpell)
                {
                    spellSelects[i].UnSelect();
                }
            }
        }       
    }

    private void ActivateWheel(InputAction.CallbackContext context)
    {
        wheel.SetActive(true);

        
        wheel.transform.SetPositionAndRotation(look.VirtualCamera.transform.position + look.VirtualCamera.transform.forward * 
                                                                             rangeFromPlayer, look.VirtualCamera.transform.rotation);

        Time.timeScale = 0;
    }

    private void DeactivateWheel(InputAction.CallbackContext context)
    {
        wheel.SetActive(false);

        Time.timeScale = 1;
    }
}
