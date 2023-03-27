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
    private bool isProjectileNotStationary = true;

    [SerializeField]
    private LayerMask possibleStationarySpawn;

    [SerializeField]
    private string objectPoolName = "Error";

    public SpellType Type { get => type; }

    public virtual void CastSpell(Player_Look player_Look, Vector3 position, Quaternion rotation, Vector3 direction)
    {
        if (isProjectileNotStationary)
        {
            Spell_Projectile spawnedProjectile = (Spell_Projectile)Object_Pooler.Pools[objectPoolName].Get();

            spawnedProjectile.Initialize(damage, effectDamage, effectBuildUp, Type, direction * travelDistance,
                                                       position, rotation, Object_Pooler.Pools[objectPoolName], destructionTime);
        }
        else
        {
            if(Physics.Raycast(position, direction, out RaycastHit hitInfo, travelDistance, possibleStationarySpawn))
            {
                Spell_Stationary stationary = (Spell_Stationary)Object_Pooler.Pools[objectPoolName].Get();

                stationary.Initialize(hitInfo.point, Quaternion.identity, Object_Pooler.Pools[objectPoolName]);
            }
        }
    }

    public enum SpellType
    {
        Fire,
        Ice,
        Earth,
        Water,
    }
}
