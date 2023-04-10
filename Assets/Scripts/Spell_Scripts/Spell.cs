using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spell")]
public class Spell : ScriptableObject
{
    //Made by Daniel.

    [SerializeField]
    protected SpellType type;

    [SerializeField]
    protected int damage;

    [SerializeField]
    protected int effectDamage = 5;

    [SerializeField]
    protected int effectBuildUp = 10;

    [Range(0.001f, 100f)]
    [SerializeField]
    private float destructionTime = 0.01f;

    [SerializeField]
    private float cooldownTime = 0.1f;

    public float CooldownTime { get => cooldownTime; }

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

    //If the spell is a projectile then a projectile is taken from the object pool and shot forward.
    //If the spell is a stationary then it is spawned where the player is looking.
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

                Quaternion stationaryRotation = Quaternion.FromToRotation(hitInfo.transform.up, hitInfo.normal) *
                                                                                                            hitInfo.transform.rotation;

                stationary.Initialize(hitInfo.point, stationaryRotation, Object_Pooler.Pools[objectPoolName], 
                                                                          damage, effectDamage, effectBuildUp, Type, destructionTime);
            }
        }
    }

    public enum SpellType
    {
        Fire,
        Ice,
        Earth,
        Water,
        Lightning,
        Wind,
        None,
    }
}
