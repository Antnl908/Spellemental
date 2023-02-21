using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spell")]
public class Spell : ScriptableObject
{
    //Made by Daniel.

    [SerializeField]
    private SpellType type;

    [SerializeField]
    private int damage;

    public SpellType Type { get => type; }

    public void CastSpell()
    {
        Debug.Log(type.ToString());
    }

    public enum SpellType
    {
        Fire,
        Ice,
        Earth,
        Water,
    }
}
