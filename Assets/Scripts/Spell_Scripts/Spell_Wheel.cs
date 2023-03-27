using System.Collections;
using System.Collections.Generic;
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

    [SerializeField]
    private float rangeFromPlayer;

    // Start is called before the first frame update
    void Start()
    {
        spellSelects.AddRange(wheel.GetComponentsInChildren<Spell_Select>());

        wheel.SetActive(false);

        controls = new();

        controls.Player1.SpellWheel.performed += ActivateWheel;
        controls.Player1.SpellWheel.canceled += DeactivateWheel;

        controls.Player1.Enable();
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
