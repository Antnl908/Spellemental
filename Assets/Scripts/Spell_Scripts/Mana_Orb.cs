using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Mana_Orb : MonoBehaviour
{
    [SerializeField]
    private Spell_Casting caster;

    [SerializeField]
    private Material material;

    [SerializeField]
    private TextMeshProUGUI manaText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        manaText.text = caster.CurrentMana + " / " + caster.MaxMana;

        material.SetFloat("_Fill", (float)caster.CurrentMana / caster.MaxMana);
    }
}
