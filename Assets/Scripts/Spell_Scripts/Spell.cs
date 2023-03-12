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
    protected int damage;

    [SerializeField]
    private int effectDamage = 5;

    [SerializeField]
    private int effectBuildUp = 10;

    [Range(0.001f, 10f)]
    [SerializeField]
    private float destructionTime = 0.01f;

    [SerializeField]
    private float travelDistance = 1f;

    [SerializeField]
    private bool isBeam = false;

    public bool IsBeam { get => isBeam; }

    [SerializeField]
    private GameObject projectile;

    public SpellType Type { get => type; }

    public virtual void CastSpell(Vector3 position, Quaternion rotation, Vector3 direction)
    {
        GameObject spawnedProjectile = Instantiate(projectile, position, rotation);

        spawnedProjectile.GetComponent<Spell_Projectile>().Initialize(damage, effectDamage, effectBuildUp, Type, direction * travelDistance);

        Destroy(spawnedProjectile, destructionTime);
    }

    public enum SpellType
    {
        Fire,
        Ice,
        Earth,
        Water,
    }
}
